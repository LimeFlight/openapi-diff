﻿using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.Compare.SchemaDiffResult
{
    public class ComposedSchemaDiffResult : SchemaDiffResult
    {
        private static RefPointer<OpenApiSchema> refPointer = new RefPointer<OpenApiSchema>(RefTypeEnum.Schemas);

        public ComposedSchemaDiffResult(OpenApiDiff openApiDiff) : base(openApiDiff)
        {
        }

        private static Dictionary<string, OpenApiSchema> GetSchema(OpenApiComponents components, Dictionary<string, string> mapping)
        {
            var result = new Dictionary<string, OpenApiSchema>();
            foreach (var map in mapping)
            {
                result.Add(map.Key, refPointer.ResolveRef(components, new OpenApiSchema(), map.Value));
            }
            return result;
        }

        private static Dictionary<string, string> GetMapping(OpenApiSchema composedSchema)
        {
            if (composedSchema.Default.AnyType != AnyType.Object)
                return null;

            var reverseMapping = new Dictionary<string, string>();
            foreach (var schema in composedSchema.OneOf)
            {
                var schemaRef = schema.Reference.ReferenceV3;
                if (schemaRef == null)
                {
                    throw new ArgumentNullException("invalid oneOf schema");
                }
                var schemaName = refPointer.GetRefName(schemaRef);
                if (schemaName == null)
                {
                    throw new ArgumentNullException("invalid schema: " + schemaRef);
                }
                reverseMapping.Add(schemaRef, schemaName);
            }

            if (composedSchema.Discriminator.Mapping != null)
            {
                foreach (var (key, value) in composedSchema.Discriminator.Mapping)
                {
                    reverseMapping.Add(value, key);
                }
            }

            return reverseMapping.ToDictionary(x => x.Value, x => x.Key);
        }

        public override ChangedSchemaBO Diff<T>(HashSet<string> refSet, OpenApiComponents leftComponents, OpenApiComponents rightComponents, T left,
            T right, DiffContextBO context)
        {
            if (left.Default.AnyType == AnyType.Object)
            {
                if (!left.OneOf.IsNullOrEmpty() || !right.OneOf.IsNullOrEmpty())
                {
                    var leftDis = left.Discriminator;
                    var rightDis = right.Discriminator;
                    if (leftDis == null
                        || rightDis == null
                        || leftDis.PropertyName == null
                        || rightDis.PropertyName == null)
                    {
                        throw new ArgumentException(
                            "discriminator or property not found for oneOf schema");
                    }

                    if (leftDis.PropertyName != rightDis.PropertyName
                        || left.OneOf.IsNullOrEmpty()
                        || right.OneOf.IsNullOrEmpty())
                    {
                        ChangedSchema.OldSchema = left;
                        ChangedSchema.NewSchema = right;
                        ChangedSchema.DiscriminatorPropertyChanged = true;
                        ChangedSchema.Context = context;
                        return ChangedSchema;
                    }

                    var leftMapping = GetMapping(left);
                    var rightMapping = GetMapping(right);

                    var mappingDiff = MapKeyDiff<string, OpenApiSchema>.Diff(GetSchema(leftComponents, leftMapping), GetSchema(rightComponents, rightMapping));
                    var changedMapping = new Dictionary<string, ChangedSchemaBO>();
                    foreach (var key in mappingDiff.SharedKey)
                    {
                        var leftSchema = new OpenApiSchema { Reference = new OpenApiReference { Id = leftMapping[key] } };
                        var rightSchema = new OpenApiSchema { Reference = new OpenApiReference { Id = rightMapping[key] } };
                        var changedSchema = OpenApiDiff.SchemaDiff
                                .Diff(refSet, leftSchema, rightSchema, context.copyWithRequired(true));
                        if (changedSchema != null)
                            changedMapping.Add(key, changedSchema);
                    }

                    ChangedSchema.OneOfSchema = new ChangedOneOfSchemaBO(leftMapping, rightMapping, context)
                    {
                        Increased = mappingDiff.Increased,
                        Missing = mappingDiff.Missing,
                        Changed = changedMapping
                    };
                }
                return base.Diff(refSet, leftComponents, rightComponents, left, right, context);
            }

            return OpenApiDiff.SchemaDiff.GetTypeChangedSchema(left, right, context);
        }
    }
}
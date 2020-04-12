using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using openapi_diff.DTOs;
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

        private Dictionary<string, OpenApiSchema> getSchema(OpenApiComponents components, Dictionary<string, string> mapping)
        {
            var result = new Dictionary<string, OpenApiSchema>();
            foreach (var map in mapping)
            {
                result.Add(map.Key, refPointer.ResolveRef(components, new OpenApiSchema(), map.Value));
            }
            return result;
        }

        private Dictionary<string, string> getMapping(ComposedSchema composedSchema)
        {
            var reverseMapping = new Dictionary<string, string>();
            foreach (var schema in composedSchema.getOneOf())
            {
                string ref = schema.get$ref ();
                if (ref == null) {
                    throw new ArgumentNullException("invalid oneOf schema");
                }
                var schemaName = refPointer.GetRefName(ref);
                if (schemaName == null)
                {
                    throw new ArgumentNullException("invalid schema: " + ref);
                }
                reverseMapping.Add(ref, schemaName);
            }

            if (composedSchema.getDiscriminator().getMapping() != null) {
                foreach (var ref in composedSchema.getDiscriminator().getMapping().keySet()) {
                    reverseMapping.Add(composedSchema.getDiscriminator().getMapping().get(ref), ref);
                }
            }

            return reverseMapping.ToDictionary(x => x.Value, x => x.Key);
        }

        public override ChangedSchemaBO diff<TV>(HashSet<string> refSet, OpenApiComponents leftComponents, OpenApiComponents rightComponents, TV left,
            TV right, DiffContextBO context)
        {
            if (left is ComposedSchema) {
                var leftComposedSchema = (ComposedSchema)left;
                var rightComposedSchema = (ComposedSchema)right;
                if (!leftComposedSchema.getOneOf().IsNullOrEmpty()
                    || !rightComposedSchema.getOneOf().IsNullOrEmpty())
                {

                    var leftDis = leftComposedSchema.getDiscriminator();
                    var rightDis = rightComposedSchema.getDiscriminator();
                    if (leftDis == null
                        || rightDis == null
                        || leftDis.getPropertyName() == null
                        || rightDis.getPropertyName() == null)
                    {
                        throw new ArgumentException(
                            "discriminator or property not found for oneOf schema");
                    }
                    else if (!leftDis.getPropertyName().equals(rightDis.getPropertyName())
                      || leftComposedSchema.getOneOf().IsNullOrEmpty()
                          || rightComposedSchema.getOneOf().IsNullOrEmpty())
                    {
                        changedSchema.OldSchema = left;
                        changedSchema.NewSchema = right;
                        changedSchema.DiscriminatorPropertyChanged = true;
                        changedSchema.Context = context;
                        return changedSchema;
                    }

                    var leftMapping = getMapping(leftComposedSchema);
                    var rightMapping = getMapping(rightComposedSchema);

                    var mappingDiff =
                        MapKeyDiff<string, OpenApiSchema>.Diff(getSchema(leftComponents, leftMapping), getSchema(rightComponents, rightMapping));
                    var changedMapping = new Dictionary<string, ChangedSchemaBO>();
                    foreach (var key in mappingDiff.SharedKey)
                    {
                        var leftSchema = new OpenApiSchema();
                        leftSchema.set$ref (leftMapping[key]);
                        var rightSchema = new OpenApiSchema();
                        rightSchema.set$ref (rightMapping[key]);
                        var changedSchema =
                            openApiDiff.SchemaDiff
                                .diff(refSet, leftSchema, rightSchema, context.copyWithRequired(true));
                        changedSchema.ifPresent(schema->changedMapping.put(key, schema));
                    }

                    changedSchema.setOneOfSchema(
                        new ChangedOneOfSchemaBO(leftMapping, rightMapping, context)
                            {
                                Increased = mappingDiff.Increased;
                            }
                            .setIncreased(mappingDiff.getIncreased())
                            .setMissing(mappingDiff.getMissing())
                            .setChanged(changedMapping));
                }   
                return base.diff(refSet, leftComponents, rightComponents, left, right, context);
            } else {
              return openApiDiff.getSchemaDiff().getTypeChangedSchema(left, right, context);
            }
        }
    }
}

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Compare;
using openapi_diff.Compare.SchemaDiffResult;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System;
using System.Collections.Generic;

namespace openapi_diff.compare
{
    public class SchemaDiff : ReferenceDiffCache<OpenApiSchema, ChangedSchemaBO>
    {
        private static RefPointer<OpenApiSchema> refPointer = new RefPointer<OpenApiSchema>(RefTypeEnum.Schemas);
        private static Dictionary<Type, Type> schemaDiffResultClassMap = new Dictionary<Type, Type>();

        private readonly OpenApiComponents leftComponents;
        private readonly OpenApiComponents rightComponents;
        private readonly OpenApiDiff _openApiDiff;

        public SchemaDiff(OpenApiDiff openApiDiff) 
        {
            schemaDiffResultClassMap.Add(typeof(OpenApiSchema), typeof(SchemaDiff));
            schemaDiffResultClassMap.Add(typeof(ArraySchema), typeof(ArraySchemaDiffResult));
            schemaDiffResultClassMap.Add(typeof(ComposedSchema), typeof(ComposedSchemaDiffResult));

            _openApiDiff = openApiDiff;
            leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }

        public static SchemaDiffResult getSchemaDiffResult(OpenApiDiff openApiDiff)
        {
            return getSchemaDiffResult(null, openApiDiff);
        }

        public static SchemaDiffResult getSchemaDiffResult(Type classType, OpenApiDiff openApiDiff)
        {
            if (classType == null)
            {
                classType = typeof(OpenApiSchema);
            }

            if (schemaDiffResultClassMap.TryGetValue(classType, out var typeEntry))
            {
                if (typeof(SchemaDiffResult).IsAssignableFrom(typeEntry))
                    return (SchemaDiffResult)Activator.CreateInstance(typeEntry, openApiDiff);
            }
            throw new ArgumentException("type " + classType + " is illegal");
        }

        protected static OpenApiSchema resolveComposedSchema(OpenApiComponents components, OpenApiSchema schema)
        {
            if (schema is ComposedSchema) {
                var composedSchema = (ComposedSchema)schema;
                List<OpenApiSchema> allOfSchemaList = composedSchema.AllOf();
                if (allOfSchemaList != null)
                {
                    foreach (var allOfSchema in allOfSchemaList)
                    {
                        allOfSchema = refPointer.resolveRef(components, allOfSchema, allOfSchema.$ref ());
                        allOfSchema = resolveComposedSchema(components, allOfSchema);
                        schema = addSchema(schema, allOfSchema);
                    }
                    composedSchema.AllOf(null);
                }
            }
            return schema;
        }

        protected static OpenApiSchema addSchema(OpenApiSchema schema, OpenApiSchema fromSchema)
        {
            if (fromSchema.Properties != null)
            {
                if (schema.Properties == null)
                {
                    schema.Properties = new Dictionary<string, OpenApiSchema>();
                }

                foreach (var property in fromSchema.Properties)
                {
                    schema.Properties.Add(property);
                }
            }

            if (fromSchema.Required != null)
            {
                if (schema.Required == null)
                {
                    schema.Required = fromSchema.Required;
                }
                else
                {
                    foreach (var required in fromSchema.Required)
                    {
                        schema.Required.Add(required);
                    }
                }
            }

            schema.ReadOnly = fromSchema.ReadOnly;
            schema.WriteOnly = fromSchema.WriteOnly;
            schema.Deprecated = fromSchema.Deprecated;
            schema.Nullable = fromSchema.Nullable;

            if (fromSchema.ExclusiveMaximum != null)
            {
                schema.ExclusiveMaximum = fromSchema.ExclusiveMaximum;
            }
            if (fromSchema.ExclusiveMinimum != null)
            {
                schema.ExclusiveMinimum = fromSchema.ExclusiveMinimum;
            }
            if (fromSchema.UniqueItems != null)
            {
                schema.UniqueItems = fromSchema.UniqueItems;
            }
            if (fromSchema.Description != null)
            {
                schema.Description = fromSchema.Description;
            }
            if (fromSchema.Format != null)
            {
                schema.Format = fromSchema.Format;
            }
            if (fromSchema.Type != null)
            {
                schema.Type = fromSchema.Type;
            }
            if (fromSchema.Enum != null)
            {
                if (schema.Enum == null)
                {
                    schema.Enum = new List<IOpenApiAny>();
                }
                //noinspection unchecked
                foreach (var element in fromSchema.Enum)
                {
                    schema.Enum.Add(element);
                }
            }
            if (fromSchema.Extensions != null)
            {
                if (schema.Extensions == null)
                {
                    schema.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                foreach (var element in fromSchema.Extensions)
                {
                    schema.Extensions.Add(element);
                }
            }
            if (fromSchema.Discriminator != null)
            {
                if (schema.Discriminator == null)
                {
                    schema.Discriminator = new OpenApiDiscriminator();
                }
                var discriminator = schema.Discriminator;
                var fromDiscriminator = fromSchema.Discriminator;

                if (fromDiscriminator.PropertyName != null)
                {
                    discriminator.PropertyName = fromDiscriminator.PropertyName;
                }
                if (fromDiscriminator.Mapping != null)
                {
                    if (discriminator.Mapping == null)
                    {
                        discriminator.Mapping  = new Dictionary<string, string>();
                    }
                    foreach (var element in fromDiscriminator.Mapping)
                    {
                        discriminator.Mapping.Add(element);
                    }
                }
            }
            if (fromSchema.Title != null)
            {
                schema.Title = fromSchema.Title;
            }
            if (fromSchema.Name() != null)
            {
                schema.Name(fromSchema.Name());
            }
            if (fromSchema.AdditionalProperties != null)
            {
                schema.AdditionalProperties = fromSchema.AdditionalProperties;
            }
            if (fromSchema.Default != null)
            {
                schema.Default = fromSchema.Default;
            }
            if (fromSchema.Example != null)
            {
                schema.Example  = fromSchema.Example;
            }
            if (fromSchema.ExternalDocs != null)
            {
                if (schema.ExternalDocs == null)
                {
                    schema.ExternalDocs = new OpenApiExternalDocs();
                }
                var externalDocs = schema.ExternalDocs;
                var fromExternalDocs = fromSchema.ExternalDocs;
                if (fromExternalDocs.Description != null)
                {
                    externalDocs.Description = fromExternalDocs.Description;
                }
                if (fromExternalDocs.Extensions != null)
                {
                    if (externalDocs.Extensions == null)
                    {
                        externalDocs.Extensions = new Dictionary<string, IOpenApiExtension>();
                    }

                    foreach (var element in fromSchema.Extensions)
                    {
                        schema.Extensions.Add(element);
                    }
                }
                if (fromExternalDocs.Url != null)
                {
                    externalDocs.Url = fromExternalDocs.Url;
                }
            }
            if (fromSchema.Maximum != null)
            {
                schema.Maximum = fromSchema.Maximum;
            }
            if (fromSchema.Minimum != null)
            {
                schema.Minimum = fromSchema.Minimum;
            }
            if (fromSchema.MaxItems != null)
            {
                schema.MaxItems = fromSchema.MaxItems;
            }
            if (fromSchema.MinItems != null)
            {
                schema.MinItems = fromSchema.MinItems;
            }
            if (fromSchema.MaxProperties != null)
            {
                schema.MaxProperties = fromSchema.MaxProperties;
            }
            if (fromSchema.MinProperties != null)
            {
                schema.MinProperties = fromSchema.MinProperties;
            }
            if (fromSchema.MaxLength != null)
            {
                schema.MaxLength = fromSchema.MaxLength;
            }
            if (fromSchema.MinLength != null)
            {
                schema.MinLength = fromSchema.MinLength;
            }
            if (fromSchema.MultipleOf != null)
            {
                schema.MultipleOf = fromSchema.MultipleOf;
            }
            if (fromSchema.Not != null)
            {
                if (schema.Not == null)
                {
                    schema.Not = addSchema(new OpenApiSchema(), fromSchema.Not);
                }
                else
                {
                    addSchema(schema.Not, fromSchema.Not);
                }
            }
            if (fromSchema.Pattern != null)
            {
                schema.Pattern = fromSchema.Pattern;
            }
            if (fromSchema.Xml != null)
            {
                if (schema.Xml == null)
                {
                    schema.Xml = new OpenApiXml();
                }
                var xml = schema.Xml;
                var fromXml  = fromSchema.Xml;

                xml.Attribute = fromXml.Attribute;

                if (fromXml.Name != null)
                {
                    xml.Name = fromXml.Name;
                }
                if (fromXml.Namespace != null)
                {
                    xml.Namespace = fromXml.Namespace;
                }
                if (fromXml.Extensions != null)
                {
                    if (xml.Extensions == null)
                    {
                        xml.Extensions = new Dictionary<string, IOpenApiExtension>();
                    }
                    foreach (var element in fromXml.Extensions)
                    {
                        xml.Extensions.Add(element);
                    }
                }
                if (fromXml.Prefix != null)
                {
                    xml.Prefix = fromXml.Prefix;
                }

                xml.Wrapped = fromXml.Wrapped;
            }
            return schema;
        }

        private static string getSchemaRef(OpenApiSchema schema)
        {
            return ofNullable(schema).map(Schema::get$ref).orElse(null);
        }

        public ChangedSchemaBO diff(HashSet<string> refSet, OpenApiSchema left, OpenApiSchema right, DiffContextBO context)
        {
            if (left == null && right == null)
            {
                return null;
            }
            return CachedDiff(refSet, left, right, getSchemaRef(left), getSchemaRef(right), context);
        }

        public ChangedSchemaBO getTypeChangedSchema(
            OpenApiSchema left, OpenApiSchema right, DiffContextBO context)
        {
            var schemaDiffResult = getSchemaDiffResult(_openApiDiff);
            schemaDiffResult.changedSchema.OldSchema = left;
            schemaDiffResult.changedSchema.NewSchema = right;
            schemaDiffResult.changedSchema.c = right;

            return 
                    .changedSchema.OldSchema = left
                    .setOldSchema(left)
                    .setNewSchema(right)
                    .setChangedType(true)
                    .setContext(context));
        }

        @Override
        protected Optional<ChangedSchema> computeDiff(
            HashSet<String> refSet, Schema left, Schema right, DiffContext context)
        {
            left = refPointer.resolveRef(this.leftComponents, left, getSchemaRef(left));
            right = refPointer.resolveRef(this.rightComponents, right, getSchemaRef(right));

            left = resolveComposedSchema(leftComponents, left);
            right = resolveComposedSchema(rightComponents, right);

            // If type of schemas are different, just set old & new schema, set changedType to true in
            // SchemaDiffResult and
            // return the object
            if ((left == null || right == null)
                || !Objects.equals(left.getType(), right.getType())
                || !Objects.equals(left.getFormat(), right.getFormat()))
            {
                return getTypeChangedSchema(left, right, context);
            }

            // If schema type is same then get specific SchemaDiffResult and compare the properties
            SchemaDiffResult result = SchemaDiff.getSchemaDiffResult(right.getClass(), openApiDiff);
            return result.diff(refSet, leftComponents, rightComponents, left, right, context);
        }
    }
}

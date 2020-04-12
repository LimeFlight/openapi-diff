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
                List<OpenApiSchema> allOfSchemaList = composedSchema.getAllOf();
                if (allOfSchemaList != null)
                {
                    foreach (var allOfSchema in allOfSchemaList)
                    {
                        allOfSchema = refPointer.resolveRef(components, allOfSchema, allOfSchema.get$ref ());
                        allOfSchema = resolveComposedSchema(components, allOfSchema);
                        schema = addSchema(schema, allOfSchema);
                    }
                    composedSchema.setAllOf(null);
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

            if (fromSchema.getRequired() != null)
            {
                if (schema.getRequired() == null)
                {
                    schema.setRequired(fromSchema.getRequired());
                }
                else
                {
                    schema.getRequired().addAll(fromSchema.getRequired());
                }
            }

            if (fromSchema.getReadOnly() != null)
            {
                schema.setReadOnly(fromSchema.getReadOnly());
            }
            if (fromSchema.getWriteOnly() != null)
            {
                schema.setWriteOnly(fromSchema.getWriteOnly());
            }
            if (fromSchema.getDeprecated() != null)
            {
                schema.setDeprecated(fromSchema.getDeprecated());
            }
            if (fromSchema.getExclusiveMaximum() != null)
            {
                schema.setExclusiveMaximum(fromSchema.getExclusiveMaximum());
            }
            if (fromSchema.getExclusiveMinimum() != null)
            {
                schema.setExclusiveMinimum(fromSchema.getExclusiveMinimum());
            }
            if (fromSchema.getNullable() != null)
            {
                schema.setNullable(fromSchema.getNullable());
            }
            if (fromSchema.getUniqueItems() != null)
            {
                schema.setUniqueItems(fromSchema.getUniqueItems());
            }
            if (fromSchema.getDescription() != null)
            {
                schema.setDescription(fromSchema.getDescription());
            }
            if (fromSchema.getFormat() != null)
            {
                schema.setFormat(fromSchema.getFormat());
            }
            if (fromSchema.getType() != null)
            {
                schema.setType(fromSchema.getType());
            }
            if (fromSchema.getEnum() != null)
            {
                if (schema.getEnum() == null)
                {
                    schema.setEnum(new ArrayList<>());
                }
                //noinspection unchecked
                schema.getEnum().addAll((List)fromSchema.getEnum());
            }
            if (fromSchema.getExtensions() != null)
            {
                if (schema.getExtensions() == null)
                {
                    schema.setExtensions(new LinkedHashMap<>());
                }
                schema.getExtensions().putAll(fromSchema.getExtensions());
            }
            if (fromSchema.getDiscriminator() != null)
            {
                if (schema.getDiscriminator() == null)
                {
                    schema.setDiscriminator(new Discriminator());
                }
                final Discriminator discriminator = schema.getDiscriminator();
                final Discriminator fromDiscriminator = fromSchema.getDiscriminator();
                if (fromDiscriminator.getPropertyName() != null)
                {
                    discriminator.setPropertyName(fromDiscriminator.getPropertyName());
                }
                if (fromDiscriminator.getMapping() != null)
                {
                    if (discriminator.getMapping() == null)
                    {
                        discriminator.setMapping(new LinkedHashMap<>());
                    }
                    discriminator.getMapping().putAll(fromDiscriminator.getMapping());
                }
            }
            if (fromSchema.getTitle() != null)
            {
                schema.setTitle(fromSchema.getTitle());
            }
            if (fromSchema.getName() != null)
            {
                schema.setName(fromSchema.getName());
            }
            if (fromSchema.getAdditionalProperties() != null)
            {
                schema.setAdditionalProperties(fromSchema.getAdditionalProperties());
            }
            if (fromSchema.getDefault() != null)
            {
                schema.setDefault(fromSchema.getDefault());
            }
            if (fromSchema.getExample() != null)
            {
                schema.setExample(fromSchema.getExample());
            }
            if (fromSchema.getExternalDocs() != null)
            {
                if (schema.getExternalDocs() == null)
                {
                    schema.setExternalDocs(new ExternalDocumentation());
                }
                final ExternalDocumentation externalDocs = schema.getExternalDocs();
                final ExternalDocumentation fromExternalDocs = fromSchema.getExternalDocs();
                if (fromExternalDocs.getDescription() != null)
                {
                    externalDocs.setDescription(fromExternalDocs.getDescription());
                }
                if (fromExternalDocs.getExtensions() != null)
                {
                    if (externalDocs.getExtensions() == null)
                    {
                        externalDocs.setExtensions(new LinkedHashMap<>());
                    }
                    externalDocs.getExtensions().putAll(fromExternalDocs.getExtensions());
                }
                if (fromExternalDocs.getUrl() != null)
                {
                    externalDocs.setUrl(fromExternalDocs.getUrl());
                }
            }
            if (fromSchema.getMaximum() != null)
            {
                schema.setMaximum(fromSchema.getMaximum());
            }
            if (fromSchema.getMinimum() != null)
            {
                schema.setMinimum(fromSchema.getMinimum());
            }
            if (fromSchema.getMaxItems() != null)
            {
                schema.setMaxItems(fromSchema.getMaxItems());
            }
            if (fromSchema.getMinItems() != null)
            {
                schema.setMinItems(fromSchema.getMinItems());
            }
            if (fromSchema.getMaxProperties() != null)
            {
                schema.setMaxProperties(fromSchema.getMaxProperties());
            }
            if (fromSchema.getMinProperties() != null)
            {
                schema.setMinProperties(fromSchema.getMinProperties());
            }
            if (fromSchema.getMaxLength() != null)
            {
                schema.setMaxLength(fromSchema.getMaxLength());
            }
            if (fromSchema.getMinLength() != null)
            {
                schema.setMinLength(fromSchema.getMinLength());
            }
            if (fromSchema.getMultipleOf() != null)
            {
                schema.setMultipleOf(fromSchema.getMultipleOf());
            }
            if (fromSchema.getNot() != null)
            {
                if (schema.getNot() == null)
                {
                    schema.setNot(addSchema(new Schema(), fromSchema.getNot()));
                }
                else
                {
                    addSchema(schema.getNot(), fromSchema.getNot());
                }
            }
            if (fromSchema.getPattern() != null)
            {
                schema.setPattern(fromSchema.getPattern());
            }
            if (fromSchema.getXml() != null)
            {
                if (schema.getXml() == null)
                {
                    schema.setXml(new XML());
                }
                final XML xml = schema.getXml();
                final XML fromXml = fromSchema.getXml();
                if (fromXml.getAttribute() != null)
                {
                    xml.setAttribute(fromXml.getAttribute());
                }
                if (fromXml.getName() != null)
                {
                    xml.setName(fromXml.getName());
                }
                if (fromXml.getNamespace() != null)
                {
                    xml.setNamespace(fromXml.getNamespace());
                }
                if (fromXml.getExtensions() != null)
                {
                    if (xml.getExtensions() == null)
                    {
                        xml.setExtensions(new LinkedHashMap<>());
                    }
                    xml.getExtensions().putAll(fromXml.getExtensions());
                }
                if (fromXml.getPrefix() != null)
                {
                    xml.setPrefix(fromXml.getPrefix());
                }
                if (fromXml.getWrapped() != null)
                {
                    xml.setWrapped(fromXml.getWrapped());
                }
            }
            return schema;
        }
    }
}

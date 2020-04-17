using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using openapi_diff.utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace openapi_diff.output
{
    public class MarkdownRender : IRender
    {
        private readonly ILogger<MarkdownRender> _logger;
        protected RefPointer<OpenApiSchema> RefPointer = new RefPointer<OpenApiSchema>(RefTypeEnum.Schemas);

        private ChangedOpenApiBO _diff;
        private const string H3 = "### ";
        private const string H4 = "#### ";
        private const string H5 = "##### ";
        private const string H6 = "###### ";
        private const string BlockQuote = "> ";
        private const string CodeText = "`";
        private const string PreCode = "    ";
        private const string PreLi = "    ";
        private const string Li = "* ";
        private const string Hr = "---\n";

        public bool ShowChangedMetadata { get; set; }

        public MarkdownRender(ILogger<MarkdownRender> logger)
        {
            _logger = logger;
        }

        public string Render(ChangedOpenApiBO diff)
        {
            _diff = diff;
            return ListEndpoints("What's New", diff.NewEndpoints)
                   + ListEndpoints("What's Deleted", diff.MissingEndpoints)
                   + ListEndpoints("What's Deprecated", diff.GetDeprecatedEndpoints())
                   + ListEndpoints(diff.ChangedOperations);
        }

        private string ListEndpoints(List<ChangedOperationBO> changedOperations)
        {
            //if (changedOperations == null || changedOperations.Count == 0) return "";
            //var sb = new StringBuilder(SectionTitle("What's Changed"));
            //changedOperations.ForEach(x =>
            //    {
            //        var details = new StringBuilder()
            //            .Append(
            //                ItemEndpoint(
            //                    x.HttpMethod.ToString(),
            //                    x.PathUrl,
            //                    x.Summary.ToString()
            //                    ));

            //        if (ChangedBO.Result(x.Parameters).IsDifferent())
            //        {
            //            details
            //                .Append(TitleH5("Parameters:"))
            //                .Append(Parameters(x.Parameters));
            //        }
            //        if (x.ResultRequestBody().IsDifferent())
            //        {
            //            details
            //                .Append(TitleH5("Request:"))
            //                .Append(Metadata("Description", x.RequestBody.Description))
            //                .Append(BodyContent(operation.RequestBody().Content()));
            //        }
            //        if (operation.resultApiResponses().isDifferent())
            //        {
            //            details
            //                .Append(TitleH5("Return Type:"))
            //                .Append(responses(operation.ApiResponses()));
            //        }

            //        sb.Append(details);
            //    });
            //return sb.ToString();
            return null;
        }
        private string ListEndpoints(string title, List<EndpointBO> endpoints)
        {
            if (endpoints == null || endpoints.Count == 0) return "";
            var sb = new StringBuilder(SectionTitle(title));
            endpoints.ForEach(x => sb.Append(ItemEndpoint(x.Method.ToString(), x.PathUrl, x.Summary)));

            return sb.ToString();
        }
        private string SectionTitle(string title)
        {
            return H4 + title + '\n' + Hr + '\n';
        }
        private string ItemEndpoint(string method, string path, string summary)
        {
            return H5 + CodeText + method + CodeText + " " + path + "\n\n" + Metadata(summary) + "\n";
        }
        private string GetBlockQuote(string beginning)
        {
            return beginning + BlockQuote;
        }
        private string GetBlockQuote(string beginning, string text)
        {
            var blockQuote = GetBlockQuote(beginning);
            return blockQuote + text.Trim().Replace("\n", "\n" + blockQuote) + '\n';
        }
        private string TitleH5(string title)
        {
            return H6 + title + '\n';
        }
 
        private string GetCode(string code)
        {
            return CodeText + code + CodeText;
        }
        private string GetMetadata(string metadata)
        {
            return GetMetadata("", metadata);
        }
        private string GetMetadata(string beginning, string metadata)
        {
            return metadata.IsNullOrEmpty() ? "" : Blockquote(beginning, metadata);
        }
        private string BodyContent(ChangedContentBO changedContent)
        {
            return BodyContent("", changedContent);
        }
        private string BodyContent(string prefix, ChangedContentBO changedContent)
        {
            if (changedContent == null)
            {
                return "";
            }
            var sb = new StringBuilder("\n");
            sb.Append(ListContent(prefix, "New content type", changedContent.Increased));
            sb.Append(ListContent(prefix, "Deleted content type", changedContent.Missing));
            var deepness = !prefix.IsNullOrEmpty() ? 1 : 0;
            var all = changedContent
                .Changed.Select(x => ItemContent(deepness, x.Key, x.Value)).ToList();
            foreach (var e in all)
                sb.Append(prefix).Append(e);

            return sb.ToString();
        }
        private string ListContent(string prefix, string title, Dictionary<string, OpenApiMediaType> mediaTypes)
        {
            var sb = new StringBuilder();

            var all = mediaTypes.Select(x => ItemContent(title, x.Key, x.Value)).ToList();
            foreach (var e in all)
                sb.Append(prefix).Append(e);

            return sb.ToString();
        }

        protected string ItemContent(string title, string mediaType)
        {
            return $"{title} : `{mediaType}` \n\n";
        }

        protected string ItemContent(string title, string mediaType, OpenApiMediaType content)
        {
            return ItemContent(title, mediaType);
        }

        protected string ItemContent(int deepness, string mediaType, ChangedMediaTypeBO content)
        {
            return ItemContent("Changed content type", mediaType) + Schema(deepness, content.Schema);
        }

        protected string Schema(ChangedSchemaBO schema)
        {
            return Schema(1, schema);
        }

        protected string Schema(int deepness, ChangedSchemaBO schema)
        {
            var sb = new StringBuilder();
            if (schema.DiscriminatorPropertyChanged)
            {
                _logger.LogDebug("Discriminator property changed");
            }
            if (schema.OneOfSchema != null)
            {
                var discriminator =
                    schema.NewSchema.Discriminator != null
                        ? schema.NewSchema.Discriminator.PropertyName
                        : "";
                sb.Append(OneOfSchema(deepness, schema.OneOfSchema, discriminator));
            }
            if (schema.Required != null)
            {
                sb.Append(Required(deepness, "New required properties", schema.Required.Increased));
                sb.Append(Required(deepness, "New optional properties", schema.Required.Missing));
            }
            if (schema.Items != null)
            {
                sb.Append(Items(deepness, schema.Items));
            }
            sb.Append(ListDiff(deepness, "enum", schema.Enumeration));
            sb.Append(
                Properties(
                    deepness,
                    "Added property",
                    schema.IncreasedProperties,
                    true,
                    schema.Context));
            sb.Append(
                Properties(
                    deepness,
                    "Deleted property",
                    schema.MissingProperties,
                    false,
                    schema.Context));
            foreach (var (name, property) in schema.ChangedProperties)
            {
                sb.Append(Property(deepness, name, property));
            }
            return sb.ToString();
        }

        protected string Schema(int deepness, OpenApiSchema schema, DiffContextBO context)
        {
            var sb = new StringBuilder();
            sb.Append(ListItem<IOpenApiAny>(deepness, "Enum", schema.Enum.ToList()));
            sb.Append(Properties(deepness, "Property", schema.Properties, true, context));

            if (schema.GetSchemaType() == SchemaTypeEnum.ComposedSchema)
            { 
                if (!schema.AllOf.IsNullOrEmpty())
                {
                    _logger.LogDebug("All of schema");
                    var all = schema.AllOf.Select(Resolve).ToList();
                    foreach (var composedChild in all)
                        sb.Append(Schema(deepness, composedChild, context));
                }
                if (!schema.OneOf.IsNullOrEmpty())
                {
                    _logger.LogDebug("One of schema");
                    sb.Append($"{Indent(deepness)}One of:\n\n");

                    var all = schema.OneOf.Select(Resolve).ToList();
                    foreach (var composedChild in all)
                        sb.Append(Schema(deepness + 1, composedChild, context));
                }
            } else if (schema.GetSchemaType() == SchemaTypeEnum.ArraySchema)
            {
                sb.Append(Items(deepness, Resolve(schema.Items), context));
            }

            return sb.ToString();
        }

        protected string OneOfSchema(int deepness, ChangedOneOfSchemaBO schema, string discriminator)
        {
            var sb = new StringBuilder();
            foreach (var (key, value) in schema.Missing)
                sb.Append($"{Indent(deepness)}Deleted '{key}' {discriminator}\n");

            foreach (var (key, sub) in schema.Increased)
                sb.Append($"{Indent(deepness)}Added '{key}' {discriminator}:\n")
                    .Append(Schema(deepness, sub, schema.Context));

            foreach (var (key, sub) in schema.Changed)
                sb.Append($"{Indent(deepness)}Updated '{key}' {discriminator}:\n")
                    .Append(Schema(deepness, sub));

            return sb.ToString();
        }

        protected string Required(int deepness, string title, List<string> required)
        {
            var sb = new StringBuilder();
            if (!required.IsNullOrEmpty())
            {
                sb.Append($"{Indent(deepness)}{title}:\n");
                foreach (var s in required)
                {
                    sb.Append($"{Indent(deepness)}- `{s}`\n");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        protected string Items(int deepness, ChangedSchemaBO schema)
        {
            var sb = new StringBuilder();
            var type = Type(schema.NewSchema);
            if (schema.IsChangedType)
            {
                type = Type(schema.OldSchema) + " -> " + Type(schema.NewSchema);
            }
            sb.Append(Items(deepness, "Changed items", type, schema.NewSchema.Description));
            sb.Append(Schema(deepness, schema));
            return sb.ToString();
        }

        protected string Items(int deepness, OpenApiSchema schema, DiffContextBO context)
        {
            return Items(deepness, "Items", Type(schema), schema.Description)
                + Schema(deepness, schema, context);
        }

        protected string Items(int deepness, string title, string type, string description)
        {
            return $"{Indent(deepness)}{title} ({type}):\n{Metadata(Indent(deepness + 1), description)}\n";
        }

        protected string Properties(
            int deepness,
            string title,
            IDictionary<string, OpenApiSchema> properties,
            bool showContent,
            DiffContextBO context)
        {
            var sb = new StringBuilder();
            if (properties != null)
            {
                foreach (var (key, value) in properties)
                {
                    sb.Append(Property(deepness, title, key, Resolve(value)));
                    if (showContent)
                    {
                        sb.Append(Schema(deepness + 1, Resolve(value), context));
                    }
                }
            }
            return sb.ToString();
        }

        protected string Property(int deepness, string name, ChangedSchemaBO schema)
        {
            var sb = new StringBuilder();
            var type = Type(schema.NewSchema);
            if (schema.IsChangedType)
            {
                type = Type(schema.OldSchema) + " -> " + Type(schema.NewSchema);
            }
            sb.Append(
                Property(deepness, "Changed property", name, type, schema.NewSchema.Description));
            sb.Append(Schema(++deepness, schema));
            return sb.ToString();
        }

        protected string Property(int deepness, string title, string name, OpenApiSchema schema)
        {
            return Property(deepness, title, name, Type(schema), schema.Description);
        }

        protected string Property(int deepness, string title, string name, string type, string description)
        {
            return $"{Indent(deepness)}* {title} `{name}` ({type})\n{Metadata(Indent(deepness + 1), description)}\n";
        }

        protected string ListDiff<T>(int deepness, string name, ChangedListBO<T> listDiff)
        {
            if (listDiff == null)
            {
                return "";
            }
            return ListItem(deepness, "Added " + name, listDiff.Increased)
                + ListItem(deepness, "Removed " + name, listDiff.Missing);
        }

        protected string ListItem<T>(int deepness, string name, List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && !list.IsNullOrEmpty())
            {
                var multiple = list.Count > 1 ? "s" : "";
                sb.Append($"{Indent(deepness)}{name} value{multiple}:\n\n");
                foreach (var p in list)
                {
                    sb.Append($"{Indent(deepness)}* `{p}`\n");
                }
            }
            return sb.ToString();
        }

        protected string Parameters(ChangedParametersBO changedParameters)
        {
            var changed = changedParameters.Changed;
            var sb = new StringBuilder("\n");
            sb.Append(ListParameter("Added", changedParameters.Increased))
                .Append(ListParameter("Deleted", changedParameters.Missing));

            var all = changed.Select(ItemParameter).ToList();
            foreach (var s in all)
                sb.Append(s);
            
            return sb.ToString();
        }

        protected string ListParameter(string title, List<OpenApiParameter> parameters)
        {
            var sb = new StringBuilder();
            var all = parameters.Select(x => ItemParameter(title, x)).ToList();
            foreach (var s in all)
                sb.Append(s);
            return sb.ToString();
        }

        protected string ItemParameter(string title, OpenApiParameter parameter)
        {
            return ItemParameter(title, parameter.Name, parameter.In.ToString(), parameter.Description);
        }

        protected string ItemParameter(string title, string name, string inCode, string description)
        {
            return $"{title}: {Code(name)} in {Code(inCode)}\n{Metadata(description)}\n";
        }

        protected string ItemParameter(ChangedParameterBO param)
        {
            var rightParam = param.NewParameter;
            if (param.IsDeprecated)
            {
                return ItemParameter("Deprecated", rightParam.Name, rightParam.In.ToString(), rightParam.Description);
            }
            return ItemParameter("Changed", rightParam.Name, rightParam.In.ToString(), rightParam.Description);
        }

        protected string Code(string str)
        {
            return CodeText + str + CodeText;
        }

        protected string Metadata(string name, ChangedMetadataBO changedMetadata)
        {
            return Metadata("", name, changedMetadata);
        }

        protected string Metadata(string beginning, string name, ChangedMetadataBO changedMetadata)
        {
            if (changedMetadata == null)
            {
                return "";
            }
            if (!ChangedUtils.IsUnchanged(changedMetadata) && ShowChangedMetadata)
            {
                return $"Changed {name}:\n{Metadata(beginning, changedMetadata.Left)}\nto:\n{Metadata(beginning, changedMetadata.Right)}\n\n";
            }
            else
            {
                return Metadata(beginning, name, changedMetadata.Right);
            }
        }

        protected string Metadata(string metadata)
        {
            return Metadata("", metadata);
        }

        protected string Metadata(string beginning, string name, string metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata))
            {
                return "";
            }
            return Blockquote(beginning, metadata);
        }

        protected string Metadata(string beginning, string metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata))
            {
                return "";
            }
            return Blockquote(beginning, metadata);
        }

        protected string Blockquote(string beginning)
        {
            return beginning + BlockQuote;
        }

        protected string Blockquote(string beginning, string text)
        {
            var blockquote = Blockquote(beginning);
            return blockquote + text.Trim().Replace("\n", "\n" + blockquote) + '\n';
        }

        protected string Type(OpenApiSchema schema)
        {
            var result = "object";
            if (schema.GetSchemaType() == SchemaTypeEnum.ArraySchema) {
                result = "array";
            } else if (schema.Type != null)
            {
                result = schema.Type;
            }
            return result;
        }

        protected string Indent(int deepness)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < deepness; i++)
            {
                sb.Append(PreLi);
            }
            return sb.ToString();
        }

        protected OpenApiSchema Resolve(OpenApiSchema schema)
        {
            return RefPointer.ResolveRef(_diff.NewSpecOpenApi.Components, schema, schema.Reference?.ReferenceV3);
        }
    }
}

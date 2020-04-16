using openapi_diff.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;
using openapi_diff.Extensions;
using openapi_diff.utils;

namespace openapi_diff.output
{
    public class MarkdownRender : IRender
    {
        private ChangedOpenApiBO _diff;
        private const string H3 = "### ";
        private const string H4 = "#### ";
        private const string H5 = "##### ";
        private const string H6 = "###### ";
        private const string BlockQuote = "> ";
        private const string Code = "`";
        private const string PreCode = "    ";
        private const string PreLi = "    ";
        private const string Li = "* ";
        private const string Hr = "---\n";

        public static bool SowChangedMetadata { get; set; }

        public string Render(ChangedOpenApiBO diff)
        {
            _diff = diff;
            return ListEndpoints("What's New", diff.NewEndpoints)
                   + ListEndpoints("What's Deleted", diff.MissingEndpoints)
                   + ListEndpoints("What's Deprecated", diff.GetDeprecatedEndpoints())
                   + ListEndpoints(diff.ChangedOperations);
        }

        private static string ListEndpoints(List<ChangedOperationBO> changedOperations)
        {
            if (changedOperations == null || changedOperations.Count == 0) return "";
            var sb = new StringBuilder(SectionTitle("What's Changed"));
            changedOperations.ForEach(x =>
                {
                    var details = new StringBuilder()
                        .Append(
                            ItemEndpoint(
                                x.HttpMethod.ToString(),
                                x.PathUrl,
                                x.Summary.ToString()
                                ));

                    if (ChangedBO.Result(x.Parameters).IsDifferent())
                    {
                        details
                            .Append(TitleH5("Parameters:"))
                            .Append(Parameters(x.Parameters));
                    }
                    if (x.ResultRequestBody().IsDifferent())
                    {
                        details
                            .Append(TitleH5("Request:"))
                            .Append(Metadata("Description", x.RequestBody.Description))
                            .Append(BodyContent(operation.RequestBody().Content()));
                    }
                    if (operation.resultApiResponses().isDifferent())
                    {
                        details
                            .append(TitleH5("Return Type:"))
                            .append(responses(operation.ApiResponses()));
                    }

                    sb.Append(details);
                });
            return sb.ToString();
        }
        private static string ListEndpoints(string title, List<EndpointBO> endpoints)
        {
            if (endpoints == null || endpoints.Count == 0) return "";
            var sb = new StringBuilder(SectionTitle(title));
            endpoints.ForEach(x => sb.Append(ItemEndpoint(x.Method.ToString(), x.PathUrl, x.Summary)));

            return sb.ToString();
        }
        private static string SectionTitle(string title)
        {
            return H4 + title + '\n' + Hr + '\n';
        }
        private static string ItemEndpoint(string method, string path, string summary)
        {
            return H5 + Code + method + Code + " " + path + "\n\n" + Metadata(summary) + "\n";
        }
        private static string Metadata(string metadata)
        {
            return Metadata("", metadata);
        }
        private static string Metadata(string name, ChangedMetadataBO changedMetadata)
        {
            return Metadata("", name, changedMetadata);
        }
        private static string Metadata(string beginning, string name, ChangedMetadataBO changedMetadata)
        {
            if (changedMetadata == null)
            {
                return "";
            }
            if (!ChangedUtils.IsUnchanged(changedMetadata) && SowChangedMetadata)
            {
                return $"Changed {name}:\n" +
                       $"{Metadata(beginning, changedMetadata.Left)}\n" +
                       "to:\n" +
                       $"{Metadata(beginning, changedMetadata.Right)}\n\n";
            }
            else
            {
                return Metadata(beginning, name, changedMetadata.Right);
            }
        }
        private static string Metadata(string beginning, string name, string metadata)
        {
            return metadata.IsNullOrEmpty() ? "" : Blockquote(beginning, metadata);
        }
        private static string Metadata(string beginning, string metadata)
        {
            return string.IsNullOrEmpty(metadata) ? "" : GetBlockQuote(beginning, metadata);
        }
        private static string GetBlockQuote(string beginning)
        {
            return beginning + BlockQuote;
        }
        private static string GetBlockQuote(string beginning, string text)
        {
            var blockQuote = GetBlockQuote(beginning);
            return blockQuote + text.Trim().Replace("\n", "\n" + blockQuote) + '\n';
        }
        private static string TitleH5(string title)
        {
            return H6 + title + '\n';
        }
        private static string Parameters(ChangedParametersBO changedParameters)
        {
            var changed = changedParameters.Changed;
            var sb = new StringBuilder("\n");
            sb.Append(ListParameter("Added", changedParameters.Increased))
                .Append(ListParameter("Deleted", changedParameters.Missing));

            foreach (var changedParameterBO in changed)
            {
                sb.Append(ItemParameter(changedParameterBO));
            }

            return sb.ToString();
        }
        private static string ListParameter(string title, List<OpenApiParameter> parameters)
        {
            var sb = new StringBuilder();
            foreach (var p in parameters)
            {
                sb.Append(ItemParameter(title, p));
            }

            return sb.ToString();
        }
        private static string ItemParameter(ChangedParameterBO param)
        {
            var rightParam = param.NewParameter;
            if (param.IsDeprecated)
            {
                return ItemParameter("Deprecated", rightParam.Name, rightParam.In.ToString(), rightParam.Description);
            }
            return ItemParameter("Changed", rightParam.Name, rightParam.ToString(), rightParam.Description);
        }
        private static string ItemParameter(string title, OpenApiParameter parameter)
        {
            return ItemParameter(
                title, parameter.Name, parameter.In.ToString(), parameter.Description);
        }
        private static string ItemParameter(string title, string name, string @in, string description)
        {
            return $"{title}:"
                   + GetCode(name)
                   + " in "
                   + GetCode(@in)
                   + '\n'
                   + GetMetadata(description)
                   + '\n';
        }
        private static string GetCode(string code)
        {
            return Code + code + Code;
        }
        private static string GetMetadata(string metadata)
        {
            return GetMetadata("", metadata);
        }
        private static string GetMetadata(string beginning, string metadata)
        {
            return metadata.IsNullOrEmpty() ? "" : Blockquote(beginning, metadata);
        }
        private static string Blockquote(string beginning, string text)
        {
            var blockquote = Blockquote(beginning);
            return blockquote + text.Trim().Replace("\n", "\n" + blockquote) + '\n';
        }
        private static string Blockquote(string beginning)
        {
            return beginning + BlockQuote;
        }
        private static string BodyContent(ChangedContentBO changedContent)
        {
            return BodyContent("", changedContent);
        }
        private static string BodyContent(string prefix, ChangedContentBO changedContent)
        {
            if (changedContent == null)
            {
                return "";
            }
            var sb = new StringBuilder("\n");
            sb.Append(listContent(prefix, "New content type", changedContent.Increased));
            sb.Append(listContent(prefix, "Deleted content type", changedContent.Missing));
            var deepness = !prefix.IsNullOrEmpty() ? 1 : 0;
            changedContent
                .Changed
                .entrySet()
                .stream()
                .map(e-> this.itemContent(deepness, e.getKey(), e.getValue()))
                .forEach(e->sb.append(prefix).append(e));
            return sb.toString();
        }
        private static string listContent(string prefix, string title, Dictionary<string, OpenApiMediaType> mediaTypes)
        {
            var sb = new StringBuilder();
            mediaTypes
                .entrySet()
                .stream()
                .map(e-> this.itemContent(title, e.getKey(), e.getValue()))
                .forEach(e->sb.append(prefix).append(e));
            return sb.toString();
        }
    }
}

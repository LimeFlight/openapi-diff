using openapi_diff.BusinessObjects;
using System.Collections.Generic;
using System.Text;

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
    }
}

namespace openapi_diff.compare
{
    public class PathsDiff
    {
        private const string RegexPath = "\\{([^/]+)\\}";
        private OpenApiDiff _openApiDiff;

        public PathsDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
        }
    }
}

using Microsoft.OpenApi.Models;

namespace openapi_diff.compare
{
    public class SchemaDiff
    {
        private OpenApiComponents leftComponents;
        private OpenApiComponents rightComponents;
        private OpenApiDiff _openApiDiff;

        public SchemaDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            leftComponents = openApiDiff.
        }
    }
}

using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Utils;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.Compare
{
    public class MetadataDiff
    {
        private OpenApiComponents _leftComponents;
        private OpenApiDiff _openApiDiff;
        private OpenApiComponents _rightComponents;

        public MetadataDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }

        public ChangedMetadataBO Diff(string left, string right, DiffContextBO context)
        {
            return ChangedUtils.IsChanged(new ChangedMetadataBO(left, right));
        }
    }
}
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.compare
{
    public class OpenApiDiff
    {
        public const string SwaggerVersionV2 = "2.0";

        private readonly ILogger<OpenApiDiff> _logger;

        public PathsDiff PathsDiff { get; set; }
        public PathDiff PathDiff { get; set; }
        public SchemaDiff SchemaDiff { get; set; }
        public ContentDiff ContentDiff { get; set; }
        public ParametersDiff ParametersDiff { get; set; }
        public ParameterDiff ParameterDiff { get; set; }
        public RequestBodyDiff RequestBodyDiff { get; set; }
        public ResponseDiff ResponseDiff { get; set; }
        public HeadersDiff HeadersDiff { get; set; }
        public HeaderDiff HeaderDiff { get; set; }
        public ApiResponseDiff APIResponseDiff { get; set; }
        public OperationDiff OperationDiff { get; set; }
        public SecurityRequirementsDiff SecurityRequirementsDiff { get; set; }
        public SecurityRequirementDiff SecurityRequirementDiff { get; set; }
        public SecuritySchemeDiff SecuritySchemeDiff { get; set; }
        public OAuthFlowsDiff OAuthFlowsDiff { get; set; }
        public OAuthFlowDiff OAuthFlowDiff { get; set; }
        public ExtensionsDiff ExtensionsDiff { get; set; }
        public MetadataDiff MetadataDiff { get; set; }

        public OpenApiDocument OldSpecOpenApi { get; set; }
        public OpenApiDocument NewSpecOpenApi { get; set; }
        public List<EndpointBO> NewEndpoints { get; set; }
        public List<EndpointBO> MissingEndpoints { get; set; }
        public List<ChangedOperationBO> ChangedOperations { get; set; }
        public ChangedExtensionsBO ChangedExtensions { get; set; }

        public OpenApiDiff(ILogger<OpenApiDiff> logger)
        {
            _logger = logger;
        }


        public string Compare(OpenApiDocument oldSpec, OpenApiDocument newSpec)
        {
            var securityRequirements = oldSpec.SecurityRequirements;

            if (securityRequirements != null)
            {
                var distinctSecurityRequirements = securityRequirements.Distinct();
            }

        }

        private void InitializeFields()
        {
            _pathsDiff = new PathsDiff(this);
            _pathDiff = new PathDiff(this);
        }
    }
}

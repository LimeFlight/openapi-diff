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
        private ContentDiff contentDiff;
        private parametersDiff;
        private ParameterDiff parameterDiff;
        private RequestBodyDiff requestBodyDiff;
        private ResponseDiff responseDiff;
        private HeadersDiff headersDiff;
        private HeaderDiff headerDiff;
        private ApiResponseDiff apiResponseDiff;
        private OperationDiff operationDiff;
        private SecurityRequirementsDiff securityRequirementsDiff;
        private SecurityRequirementDiff securityRequirementDiff;
        private SecuritySchemeDiff securitySchemeDiff;
        private OAuthFlowsDiff oAuthFlowsDiff;
        private OAuthFlowDiff oAuthFlowDiff;
        private ExtensionsDiff extensionsDiff;
        private MetadataDiff metadataDiff;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SharpYaml.Serialization.Logging;

namespace openapi_diff.compare
{
    public class OpenApiDiff
    {
        public const string SwaggerVersionV2 = "2.0";

        private readonly ILogger<OpenApiDiff> _logger;

        private PathsDiff pathsDiff;
        private PathDiff pathDiff;
        private SchemaDiff schemaDiff;
        private ContentDiff contentDiff;
        private  parametersDiff;
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

        private OpenAPI oldSpecOpenApi;
        private OpenAPI newSpecOpenApi;
        private List<Endpoint> newEndpoints;
        private List<Endpoint> missingEndpoints;
        private List<ChangedOperation> changedOperations;
        private ChangedExtensions changedExtensions;

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

using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class ChangedOperationDTO
    {
        public OpenApiOperation OldOperation { get; set; }
        public OpenApiOperation NewOperation { get; set; }
        public string PathUrl { get; set; }
        public OperationType HttpMethod { get; set; }
        public ChangedMetadataDTO Summary { get; set; }
        public ChangedMetadataDTO Description { get; set; }
        public bool Deprecated { get; set; }
        public ChangedParametersDTO Parameters { get; set; }
        public ChangedRequestBodyDTO RequestBody { get; set; }
        public ChangedAPIResponseDTO APIResponses { get; set; }
        public ChangedSecurityRequirementsDTO SecurityRequirements { get; set; }
        public ChangedExtensionsDTO Extensions { get; set; }
    }
}

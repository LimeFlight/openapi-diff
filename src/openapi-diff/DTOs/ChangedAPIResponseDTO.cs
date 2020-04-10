using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class ChangedAPIResponseDTO
    {
        public Dictionary<string, OpenApiResponse> Increased { get; set; }
        public Dictionary<string, OpenApiResponse> Missing { get; set; }
        public Dictionary<string, ChangedResponseDTO> Changed { get; set; }
        public ChangedExtensionsDTO Extensions { get; set; }
    }
}

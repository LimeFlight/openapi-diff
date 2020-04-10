using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace openapi_diff.DTOs
{
    public class ChangedHeadersDTO
    {
        private Dictionary<string, OpenApiHeader> Increased { get; set; }
        private Dictionary<string, OpenApiHeader> Missing { get; set; }
        private Dictionary<string, ChangedHeadersDTO> Changed { get; set; }
    }
}

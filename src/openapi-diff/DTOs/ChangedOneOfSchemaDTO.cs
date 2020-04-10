using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace openapi_diff.DTOs
{
    public class ChangedOneOfSchemaDTO
    {
        private Dictionary<string, OpenApiSchema> Increased { get; set; }
        private Dictionary<string, OpenApiSchema> Missing { get; set; }
        private Dictionary<string, ChangedSchemaDTO> Changed { get; set; }
    }
}

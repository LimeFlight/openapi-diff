using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class ChangedSecurityRequirementsDTO
    {
        private List<OpenApiSecurityRequirement> OldSecurityRequirements { get; set; }
        private List<OpenApiSecurityRequirement> NewSecurityRequirements { get; set; }
        private List<OpenApiSecurityRequirement> Missing { get; set; }
        private List<OpenApiSecurityRequirement> Increased { get; set; }
        private List<ChangedSecurityRequirementsDTO> Changed { get; set; }
    }
}

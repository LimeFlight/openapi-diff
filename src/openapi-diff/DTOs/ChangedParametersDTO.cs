using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace openapi_diff.DTOs
{
    public class ChangedParametersDTO
    {
        public List<OpenApiParameter> OldParameterList { get; set; }
        public List<OpenApiParameter> NewParameterList { get; set; }
        public DiffContextDTO Context { get; set; }
        public List<OpenApiParameter> Increased { get; set; }
        public List<OpenApiParameter> Missing { get; set; }
        public List<ChangedParametersDTO> Changed { get; set; }
    }
}

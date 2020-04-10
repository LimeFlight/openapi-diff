using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class ChangedContentDTO
    {
        private Dictionary<string, OpenApiMediaType> increased;
        private Dictionary<string, OpenApiMediaType> missing;
        private Dictionary<string, ChangedMediaTypeDTO> changed;
    }
}

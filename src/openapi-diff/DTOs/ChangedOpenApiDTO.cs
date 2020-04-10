using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace openapi_diff.DTOs
{
    public class ChangedOpenApiDTO
    {
        private OpenApiDocument OldSpecOpenApi { get; set; }
        public OpenApiDocument NewSpecOpenApi { get; set; }
        public List<EndpointDTO> NewEndpoints { get; set; }
        public List<EndpointDTO> MissingEndpoints { get; set; }
        public List<ChangedOperationDTO> ChangedOperations { get; set; }
        public ChangedExtensionsDTO ChangedExtensions { get; set; }

        public bool isCompatible()
        {
            throw new NotImplementedException();
        }

        public bool isIncompatible()
        {
            throw new NotImplementedException();
        }

        public bool isUnchanged()
        {
            throw new NotImplementedException();
        }
    }
}

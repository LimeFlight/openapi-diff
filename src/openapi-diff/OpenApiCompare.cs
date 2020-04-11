using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using openapi_diff.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using openapi_diff.BusinessObjects;

namespace openapi_diff
{
    public class OpenAPICompare : IOpenAPICompare
    {
        public ChangedOpenApiBO FromLocations(string oldLocation, string newLocation)
        {
            return FromSpecifications(ReadLocation(oldLocation), ReadLocation(newLocation));
        }

        public ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec)
        {
            return new ChangedOpenApiBO();
        }

        private static OpenApiDocument ReadLocation(string location, List<OpenApiOAuthFlow> auths = null)
        {
            using var sr = new StreamReader(location);

            var openAPIDoc =  new OpenApiStreamReader().Read(sr.BaseStream, out var diagnostic);

            if (diagnostic.Errors != null)
                throw new Exception("Error reading file");

            return openAPIDoc;
        }
    }
}

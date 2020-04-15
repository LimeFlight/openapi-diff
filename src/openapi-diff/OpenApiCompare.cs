using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using openapi_diff.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using openapi_diff.Extensions;

namespace openapi_diff
{
    public class OpenAPICompare : IOpenAPICompare
    {
        private readonly ILogger<OpenAPICompare> _logger;
        private readonly IEnumerable<IExtensionDiff> _extensions;

        public OpenAPICompare(ILogger<OpenAPICompare> logger, IEnumerable<IExtensionDiff> extensions)
        {
            _logger = logger;
            _extensions = extensions;
        }

        public ChangedOpenApiBO FromLocations(string oldLocation, string newLocation)
        {
            return FromSpecifications(ReadLocation(oldLocation), ReadLocation(newLocation));
        }

        public ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec)
        {
            return OpenApiDiff.Compare(oldSpec, newSpec, _extensions, _logger);
        }

        private static OpenApiDocument ReadLocation(string location, List<OpenApiOAuthFlow> auths = null)
        {
            using var sr = new StreamReader(location);

            var openAPIDoc =  new OpenApiStreamReader().Read(sr.BaseStream, out var diagnostic);

            if (!diagnostic.Errors.IsNullOrEmpty())
                throw new Exception("Error reading file");

            return openAPIDoc;
        }
    }
}

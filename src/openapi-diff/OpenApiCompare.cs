using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using openapi_diff.BusinessObjects;
using openapi_diff.compare;
using openapi_diff.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

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

        public ChangedOpenApiBO FromLocations(string oldLocation, string newLocation, OpenApiReaderSettings settings = null)
        {
            return FromSpecifications(ReadLocation(oldLocation, settings: settings), ReadLocation(newLocation, settings: settings));
        }

        public ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec)
        {
            return OpenApiDiff.Compare(oldSpec, newSpec, _extensions, _logger);
        }

        private static OpenApiDocument ReadLocation(string location, List<OpenApiOAuthFlow> auths = null, OpenApiReaderSettings settings = null)
        {
            using var sr = new StreamReader(location);

            var openAPIDoc =  new OpenApiStreamReader(settings).Read(sr.BaseStream, out var diagnostic);
            if (!diagnostic.Errors.IsNullOrEmpty())
                throw new Exception($"Error reading file. Error: {string.Join(", ", diagnostic.Errors)}");

            return openAPIDoc;
        }
    }
}

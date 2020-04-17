using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using openapi_diff.BusinessObjects;

namespace openapi_diff
{
    public interface IOpenAPICompare
    {
        ChangedOpenApiBO FromLocations(string oldLocation, string newLocation, OpenApiReaderSettings settings = null);
        ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec);
    }
}

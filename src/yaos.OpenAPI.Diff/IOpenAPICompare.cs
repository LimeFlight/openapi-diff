using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using yaos.OpenAPI.Diff.BusinessObjects;

namespace yaos.OpenAPI.Diff
{
    public interface IOpenAPICompare
    {
        ChangedOpenApiBO FromLocations(string oldLocation, string newLocation, OpenApiReaderSettings settings = null);
        ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec);
    }
}

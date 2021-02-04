using LimeFlight.OpenAPI.Diff.BusinessObjects;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace LimeFlight.OpenAPI.Diff
{
    public interface IOpenAPICompare
    {
        ChangedOpenApiBO FromLocations(string oldLocation, string newLocation, OpenApiReaderSettings settings = null);
        ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, string oldSpecIdentifier, OpenApiDocument newSpec, string newSpecIdentifier);
    }
}

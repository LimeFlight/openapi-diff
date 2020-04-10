using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;

namespace openapi_diff
{
    public interface IOpenAPICompare
    {
        ChangedOpenApiDTO FromLocations(string oldLocation, string newLocation);
        ChangedOpenApiDTO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec);
    }
}

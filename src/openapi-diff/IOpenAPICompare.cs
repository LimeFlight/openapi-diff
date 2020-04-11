using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;

namespace openapi_diff
{
    public interface IOpenAPICompare
    {
        ChangedOpenApiBO FromLocations(string oldLocation, string newLocation);
        ChangedOpenApiBO FromSpecifications(OpenApiDocument oldSpec, OpenApiDocument newSpec);
    }
}

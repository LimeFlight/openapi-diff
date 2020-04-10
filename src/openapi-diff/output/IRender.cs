using openapi_diff.DTOs;

namespace openapi_diff.output
{
    public interface IRender
    {
        string Render(ChangedOpenApiDTO diff);
    }
}

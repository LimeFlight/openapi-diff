using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;

namespace openapi_diff.output
{
    public interface IRender
    {
        string Render(ChangedOpenApiBO diff);
    }
}

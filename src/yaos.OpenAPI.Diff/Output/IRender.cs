using yaos.OpenAPI.Diff.BusinessObjects;

namespace yaos.OpenAPI.Diff.Output
{
    public interface IRender
    {
        string Render(ChangedOpenApiBO diff);
    }
}

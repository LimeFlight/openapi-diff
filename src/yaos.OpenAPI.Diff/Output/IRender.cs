using System.Threading.Tasks;
using yaos.OpenAPI.Diff.BusinessObjects;

namespace yaos.OpenAPI.Diff.Output
{
    public interface IRender
    {
        Task<string> Render(ChangedOpenApiBO diff);
    }
}

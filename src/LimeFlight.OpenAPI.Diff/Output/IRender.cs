using System.Threading.Tasks;
using LimeFlight.OpenAPI.Diff.BusinessObjects;

namespace LimeFlight.OpenAPI.Diff.Output
{
    public interface IRender
    {
        Task<string> Render(ChangedOpenApiBO diff);
    }
}
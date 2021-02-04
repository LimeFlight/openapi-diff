using System.Threading.Tasks;
using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Extensions;
using RazorLight;

namespace LimeFlight.OpenAPI.Diff.Output.Html
{
    public class HtmlRender : BaseRenderer, IHtmlRender
    {
        private readonly RazorLightEngine _engine;
        private readonly string _title;

        public HtmlRender()
        {
            _engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(HtmlRender))
                .UseMemoryCachingProvider()
                .Build();
        }

        public HtmlRender(string title) : this()
        {
            _title = title;
        }

        public async Task<string> Render(ChangedOpenApiBO diff)
        {
            var model = !_title.IsNullOrEmpty() ? GetRenderModel(diff, _title) : GetRenderModel(diff);
            return await _engine.CompileRenderAsync("Views.Index", model);
        }
    }
}
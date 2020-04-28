﻿using System.Threading.Tasks;
using RazorLight;
using yaos.OpenAPI.Diff.BusinessObjects;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.Output.Html
{
    public class HtmlRender : BaseRenderer, IHtmlRender
    {
        private readonly string _title;
        private readonly RazorLightEngine _engine;

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
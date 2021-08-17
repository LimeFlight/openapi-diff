using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Extensions;
using Microsoft.CodeAnalysis;
using RazorLight;

namespace LimeFlight.OpenAPI.Diff.Output.Html
{
    public class HtmlRender : BaseRenderer, IHtmlRender
    {
        private readonly RazorLightEngine _engine;
        private readonly string _title;

        public HtmlRender()
        {
            // Get the directory of a core assembly. We need this directory to
            // build out our platform specific reference to mscorlib. mscorlib
            // and the private mscorlib must be supplied as references for
            // compilation to succeed. Of these two assemblies, only the private
            // mscorlib is discovered via enumerataing assemblies referenced by
            // this executing binary.
            var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
            var coreDir = Directory.GetParent(dd);

            var metadataReference = new List<MetadataReference>
            {   
                // Here we get the path to the mscorlib and private mscorlib
                // libraries that are required for compilation to succeed.
                MetadataReference.CreateFromFile(coreDir.FullName + Path.DirectorySeparatorChar + "mscorlib.dll"),
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location)
            };

            _engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(HtmlRender))
                .UseMemoryCachingProvider()
                .AddMetadataReferences(metadataReference.ToArray())
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
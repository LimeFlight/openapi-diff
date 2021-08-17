using System;
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
            var builder = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(HtmlRender))
                .UseMemoryCachingProvider();

            // workaround to be able to compile Razor templates in self-contained deployments 
            Console.WriteLine($"Start metadata workaround");

            var coreDir = Directory.GetParent(typeof(Enumerable).GetTypeInfo().Assembly.Location);
            var myDir = Directory.GetParent(GetType().GetTypeInfo().Assembly.Location);

            Console.WriteLine($"Core dir path: {coreDir}");
            Console.WriteLine($"My dir path: {myDir}");

            Console.WriteLine($"Add metadata");
            var coreMetaDataPath = coreDir.FullName + Path.DirectorySeparatorChar + "mscorlib.dll";
            var myDirMetaDataPath = typeof(object).GetTypeInfo().Assembly.Location;

            builder.AddMetadataReferences(
                MetadataReference.CreateFromFile(coreMetaDataPath),
                MetadataReference.CreateFromFile(myDirMetaDataPath));

            Console.WriteLine($"Added metadata: {coreMetaDataPath}");
            Console.WriteLine($"Added metadata: {myDirMetaDataPath}");

            //// Enumerate all assemblies referenced by this executing assembly
            //// and provide them as references to the build script we're about to
            //// compile.
            //var referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            //foreach (var referencedAssembly in referencedAssemblies)
            //{
            //    var loadedAssembly = Assembly.Load(referencedAssembly);
            //    var loadedAssemblyPath = loadedAssembly.Location;
            //    Console.WriteLine($"Added metadata: {loadedAssemblyPath}");
            //    builder.AddMetadataReferences(MetadataReference.CreateFromFile(loadedAssemblyPath));
            //}

            _engine = builder
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
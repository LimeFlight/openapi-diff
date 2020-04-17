using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using yaos.OpenAPI.Diff.Compare;

namespace yaos.OpenAPI.Diff.CLI
{
    class Program
    {
        [Required]
        [Option(CommandOptionType.SingleValue, ShortName = "o", LongName = "old", Description = "Path to old OpenAPI Specification file")]
        public string OldPath { get; }
        [Required]
        [Option(CommandOptionType.SingleValue, ShortName = "n", LongName = "new", Description = "Path to new OpenAPI Specification file")]
        public string NewPath { get; }

        static void Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);
        
        private void OnExecute()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddFilter("", LogLevel.Trace);
                    builder.AddConsole();
                })
                .AddSingleton<IOpenAPICompare, OpenAPICompare>()
                .AddTransient(x => (IExtensionDiff)x.GetService(typeof(ExtensionDiff)))
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogDebug("Starting application");

            if (OldPath == null || NewPath == null)
            {
                var versionString = Assembly.GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

                Console.WriteLine($"openapi-diff-cli v{versionString}");
                Console.WriteLine("-------------");
                Console.WriteLine("\nUsage:");
                Console.WriteLine("  openapi-diff-cli --old <paht> --new <path>");
                return;
            }

            var openAPICompare = serviceProvider.GetService<IOpenAPICompare>();
            openAPICompare.FromLocations(OldPath, NewPath);

            logger.LogDebug("All done!");
        }
    }
}

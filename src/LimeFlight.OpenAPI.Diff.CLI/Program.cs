﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using LimeFlight.OpenAPI.Diff.Compare;
using LimeFlight.OpenAPI.Diff.Output;
using LimeFlight.OpenAPI.Diff.Output.Html;
using LimeFlight.OpenAPI.Diff.Output.Markdown;

namespace LimeFlight.OpenAPI.Diff.CLI
{
    class Program
    {
        private ILogger _logger;

        [Required]
        [Option(CommandOptionType.SingleValue, ShortName = "o", LongName = "old", Description = "Path to old OpenAPI Specification file")]
        public string OldPath { get; }
        [Required]
        [Option(CommandOptionType.SingleValue, ShortName = "n", LongName = "new", Description = "Path to new OpenAPI Specification file")]
        public string NewPath { get; }

        [Option(CommandOptionType.SingleValue, ShortName = "e", LongName = "exit", Description = "Define exit behavior. Default: Fail only if API changes broke backward compatibility")]
        public ExitTypeEnum? ExitType { get; }

        [Option(CommandOptionType.SingleValue, ShortName = "m", LongName = "markdown", Description = "Export diff as markdown in given file")]
        public string Markdown { get; }

        [Option(CommandOptionType.NoValue, ShortName = "c", LongName = "console", Description = "Export diff in console")]
        public bool ToConsole { get; }

        [Option(CommandOptionType.SingleValue, ShortName = "h", LongName = "html", Description = "Export diff as html in given file")]
        public string HTML { get; }

        static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        private async Task<int> OnExecute()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddFilter("", LogLevel.Trace);
                    builder.AddConsole();
                })
                .AddSingleton<IOpenAPICompare, OpenAPICompare>()
                .AddSingleton<IMarkdownRender, MarkdownRender>()
                .AddSingleton<IHtmlRender, HtmlRender>()
                .AddSingleton<IConsoleRender, ConsoleRender>()
                .AddTransient(x => (IExtensionDiff)x.GetService(typeof(ExtensionDiff)))
                .BuildServiceProvider();

            _logger = serviceProvider.GetService<ILogger<Program>>();
            _logger.LogDebug("Starting application");

            var openAPICompare = serviceProvider.GetService<IOpenAPICompare>();
            var result = openAPICompare.FromLocations(OldPath, NewPath);
            if (ToConsole)
            {
                var renderer = serviceProvider.GetService<IConsoleRender>();
                var renderedResult = await renderer.Render(result);
                Console.WriteLine(renderedResult);
            }
            if (HTML != null && Uri.IsWellFormedUriString(HTML, UriKind.RelativeOrAbsolute))
            {
                var renderer = serviceProvider.GetService<IHtmlRender>();
                var renderedResult = await renderer.Render(result);
                SaveToFile(renderedResult, HTML);
            }
            if (Markdown != null && Uri.IsWellFormedUriString(Markdown, UriKind.RelativeOrAbsolute))
            {
                var renderer = serviceProvider.GetService<IMarkdownRender>();
                var renderedResult = await renderer.Render(result);
                SaveToFile(renderedResult, Markdown);
            }

            switch (ExitType)
            {
                case ExitTypeEnum.PrintState:
                    Console.WriteLine(result.IsChanged().DiffResult);
                    break;
                case ExitTypeEnum.FailOnChanged:
                    Environment.ExitCode = result.IsUnchanged() ? 0 : 1;
                    break;
                case null:
                    Environment.ExitCode = result.IsCompatible() ? 0 : 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _logger.LogDebug("All done!");
            return Environment.ExitCode;
        }

        private void SaveToFile(string renderedResult, string path)
        {
            try
            {
                var file = new FileInfo(path);
                file.Directory?.Create(); // If the directory already exists, this method does nothing.

                Console.WriteLine($"Writing html report to: {path}");
                File.WriteAllText(path, renderedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
    }
}

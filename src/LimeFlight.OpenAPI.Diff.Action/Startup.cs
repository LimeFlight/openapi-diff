using System;
using LimeFlight.OpenAPI.Diff.Output.Html;
using LimeFlight.OpenAPI.Diff.Output.Markdown;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LimeFlight.OpenAPI.Diff.Action
{
    public static class Startup
    {
        public static IServiceProvider Build()
        {
            return new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddFilter("", LogLevel.Trace);
                    builder.AddConsole();
                })
                .AddSingleton<IOpenAPICompare, OpenAPICompare>()
                .AddSingleton<IMarkdownRender, MarkdownRender>()
                .AddSingleton<IHtmlRender, HtmlRender>()
                .BuildServiceProvider();
        }
    }
}

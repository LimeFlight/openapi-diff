using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using yaos.OpenAPI.Diff;
using yaos.OpenAPI.Diff.Compare;
using yaos.OpenAPI.Diff.Tests;

namespace yaos.OpenApi.Diff.Tests._Base
{
    public class BaseTest
    {
        public readonly ITestUtils TestUtils;
        public readonly ITestOutputHelper OutputHelper;

        public BaseTest()
        {
            var services = new ServiceCollection();
            services.AddTransient<ITestUtils, TestUtils>();
            services.AddTransient<IOpenAPICompare, OpenAPICompare>();
            services.AddLogging();
            services.AddTransient(x => (IExtensionDiff) x.GetService(typeof(ExtensionDiff)));
            
            var serviceProvider = services.BuildServiceProvider();
            
            TestUtils = serviceProvider.GetService<ITestUtils>();
            OutputHelper = serviceProvider.GetService<ITestOutputHelper>();
        }
    }
}

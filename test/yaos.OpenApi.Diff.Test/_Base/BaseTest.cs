using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using openapi_diff;
using openapi_diff.compare;
using Xunit.Abstractions;
using Xunit.Sdk;
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

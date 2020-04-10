using Microsoft.Extensions.DependencyInjection;
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

            var serviceProvider = services.BuildServiceProvider();

            TestUtils = serviceProvider.GetService<ITestUtils>();
            OutputHelper = serviceProvider.GetService<ITestOutputHelper>();
        }
    }
}

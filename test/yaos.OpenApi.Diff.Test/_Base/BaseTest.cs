using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
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
            services.RegisterAll<IExtensionDiff>(new[] {GetType().Assembly}, ServiceLifetime.Transient);
            
            var serviceProvider = services.BuildServiceProvider();
            
            TestUtils = serviceProvider.GetService<ITestUtils>();
            OutputHelper = serviceProvider.GetService<ITestOutputHelper>();
        }

       
    }

    public static class ServiceCollectionExtension {
        public static void RegisterAll<T>(this IServiceCollection serviceCollection, Assembly[] assemblies, ServiceLifetime lifetime)
        {
            var typesFromAssemblies = assemblies
                .SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));

            foreach (var type in typesFromAssemblies)
                serviceCollection.Add(new ServiceDescriptor(typeof(T), type, lifetime));
        }
    }
}

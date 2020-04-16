using System;
using System.Reflection;

namespace openapi_diff
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var versionString = Assembly.GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

                Console.WriteLine($"openapi-diff v{versionString}");
                Console.WriteLine("-------------");
                Console.WriteLine("\nUsage:");
                Console.WriteLine("  openapi-diff <message>");
                return;
            }
        }
    }
}

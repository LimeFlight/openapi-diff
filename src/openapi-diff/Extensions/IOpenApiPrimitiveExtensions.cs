using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Writers;
using System.IO;

namespace openapi_diff.Extensions
{
    public static class IOpenApiPrimitiveExtensions
    {
        public static string GetValueString(this IOpenApiPrimitive primitive)
        {
            using var sb = new StringWriter();
            var writer = new OpenApiYamlWriter(sb);
            primitive.Write(writer, OpenApiSpecVersion.OpenApi3_0);
            return sb.ToString();
        }
    }
}

using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class EndpointDTO
    {
        public string PathUrl { get; set; }
        public OperationType Method { get; set; }
        public string Summary { get; set; }
        public OpenApiPathItem Path { get; set; }
        public OpenApiOperation Operation { get; set; }
    }
}

using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public static class EndpointBO
    {
        public static string ToString(this EndpointDTO endpoint)
        {
            return endpoint.Method + " " + endpoint.PathUrl;
        }
    }
}

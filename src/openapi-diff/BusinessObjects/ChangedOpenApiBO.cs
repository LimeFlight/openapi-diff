using System.Collections.Generic;
using System.Linq;
using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public static class ChangedOpenApiBO
    {
        public static List<EndpointDTO> GetDeprecatedEndpoints(this ChangedOpenApiDTO changedOpenApi)
        {
            var tmp = new List<EndpointDTO>();
            changedOpenApi.ChangedOperations
                .Where(x => x.Deprecated)
                .ToList()
                .ForEach(x => tmp.Add(x.ConvertToEndpoint()));

            return tmp;
        }
    }
}

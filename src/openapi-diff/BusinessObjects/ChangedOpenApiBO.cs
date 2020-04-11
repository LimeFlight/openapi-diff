using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using openapi_diff.DTOs;
using openapi_diff.Extensions;

namespace openapi_diff.BusinessObjects
{
    public class ChangedOpenApiBO : ComposedChangedBO
    {
        private OpenApiDocument _oldSpecOpenApi;
        private OpenApiDocument _newSpecOpenApi;

        public List<EndpointBO> NewEndpoints { get; set; }
        public List<EndpointBO> MissingEndpoints { get; set; }
        public List<ChangedOperationBO> ChangedOperations { get; set; }
        public ChangedExtensionsBO ChangedExtensions { get; set; }

        public List<EndpointBO> GetDeprecatedEndpoints()
        {
            return ChangedOperations
                .Where(x => x.IsDeprecated)
                .Select(x => x.ConvertToEndpoint())
                .ToList();
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(ChangedOperations){ ChangedExtensions };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (NewEndpoints.IsNullOrEmpty() && MissingEndpoints.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (MissingEndpoints.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

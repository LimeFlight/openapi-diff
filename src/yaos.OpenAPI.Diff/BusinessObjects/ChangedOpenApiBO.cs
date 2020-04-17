using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedOpenApiBO : ComposedChangedBO
    {
        public OpenApiDocument OldSpecOpenApi { get; set; }
        public OpenApiDocument NewSpecOpenApi { get; set; }
        public List<EndpointBO> NewEndpoints { get; set; }
        public List<EndpointBO> MissingEndpoints { get; set; }
        public List<ChangedOperationBO> ChangedOperations { get; set; }
        public ChangedExtensionsBO ChangedExtensions { get; set; }

        public ChangedOpenApiBO()
        {
            NewEndpoints = new List<EndpointBO>();
            MissingEndpoints = new List<EndpointBO>();
            ChangedOperations = new List<ChangedOperationBO>();
        }
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

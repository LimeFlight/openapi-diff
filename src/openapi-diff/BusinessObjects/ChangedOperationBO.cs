using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public class ChangedOperationBO : ComposedChangedBO
    {
        public OpenApiOperation OldOperation { get; set; }
        public OpenApiOperation NewOperation { get; set; }
        public string PathUrl { get; set; }
        public OperationType HttpMethod { get; set; }
        public ChangedMetadataBO Summary { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public bool IsDeprecated { get; set; }
        public ChangedParametersBO Parameters { get; set; }
        public ChangedRequestBodyBO RequestBody { get; set; }
        public ChangedAPIResponseBO APIResponses { get; set; }
        public ChangedSecurityRequirementsBO SecurityRequirements { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public EndpointBO ConvertToEndpoint()
        {
            var endpoint = new EndpointBO
            {
                PathUrl = PathUrl,
                Method = HttpMethod,
                Summary = NewOperation.Summary,
                Operation = NewOperation
            };
            return endpoint;
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>
            {
                Summary,
                Description,
                Parameters,
                RequestBody,
                APIResponses,
                SecurityRequirements,
                Extensions
            };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (IsDeprecated)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }

        public DiffResultBO ResultApiResponses()
        {
            return Result(APIResponses);
        }

        public DiffResultBO ResultRequestBody()
        {
            return RequestBody == null ? new DiffResultBO(DiffResultEnum.NoChanges) : RequestBody.IsChanged();
        }
    }
}

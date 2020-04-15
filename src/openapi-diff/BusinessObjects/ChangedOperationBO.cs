using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public class ChangedOperationBO : ComposedChangedBO
    {
        private readonly OpenApiOperation _oldOperation;
        private readonly OpenApiOperation _newOperation;

        public OperationType HttpMethod { get; }
        public string PathUrl { get; }
        public ChangedMetadataBO Summary { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public bool IsDeprecated { get; set; }
        public ChangedParametersBO Parameters { get; set; }
        public ChangedRequestBodyBO RequestBody { get; set; }
        public ChangedAPIResponseBO APIResponses { get; set; }
        public ChangedSecurityRequirementsBO SecurityRequirements { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedOperationBO(string pathUrl, OperationType httpMethod, OpenApiOperation oldOperation, OpenApiOperation newOperation)
        {
            PathUrl = pathUrl;
            HttpMethod = httpMethod;
            _oldOperation = oldOperation;
            _newOperation = newOperation;
        }

        public EndpointBO ConvertToEndpoint()
        {
            var endpoint = new EndpointBO
            {
                PathUrl = PathUrl,
                Method = HttpMethod,
                Summary = _newOperation.Summary,
                Operation = _newOperation
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

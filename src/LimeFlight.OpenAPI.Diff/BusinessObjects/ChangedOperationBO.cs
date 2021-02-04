using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedOperationBO : ComposedChangedBO
    {
        public ChangedOperationBO(string pathUrl, OperationType httpMethod, OpenApiOperation oldOperation,
            OpenApiOperation newOperation)
        {
            PathUrl = pathUrl;
            HttpMethod = httpMethod;
            OldOperation = oldOperation;
            NewOperation = newOperation;
        }

        public OpenApiOperation OldOperation { get; }
        public OpenApiOperation NewOperation { get; }

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

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.Operation;
        }

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

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>
                {
                    ("Summary", Summary),
                    ("Description", Description),
                    ("Parameters", Parameters),
                    ("RequestBody", RequestBody),
                    ("Responses", APIResponses),
                    ("SecurityRequirements", SecurityRequirements),
                    (null, Extensions)
                }
                .Where(x => x.Change != null).ToList();
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (IsDeprecated) return new DiffResultBO(DiffResultEnum.Compatible);
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            var returnList = new List<ChangedInfoBO>();
            var elementType = GetElementType();
            const TypeEnum changeType = TypeEnum.Changed;

            if (IsDeprecated)
                returnList.Add(new ChangedInfoBO(elementType, changeType, "Deprecation",
                    OldOperation?.Deprecated.ToString(), NewOperation?.Deprecated.ToString()));

            return returnList;
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
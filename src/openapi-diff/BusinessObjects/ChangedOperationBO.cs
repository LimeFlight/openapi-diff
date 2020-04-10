using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public static class ChangedOperationBO
    {
        public static EndpointDTO ConvertToEndpoint(this ChangedOperationDTO changedOperation)
        {
            var endpoint = new EndpointDTO
            {
                PathUrl = changedOperation.PathUrl,
                Method = changedOperation.HttpMethod,
                Summary = changedOperation.NewOperation.Summary,
                Operation = changedOperation.NewOperation
            };
            return endpoint;
        }
    }
}

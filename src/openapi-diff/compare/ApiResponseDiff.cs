using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Compare;
using openapi_diff.utils;
using System.Collections.Generic;

namespace openapi_diff.compare
{
    public class ApiResponseDiff
    {
        private readonly OpenApiDiff _openApiDiff;

        public ApiResponseDiff(OpenApiDiff openApiDiff)
        {
            this._openApiDiff = openApiDiff;
        }

        public ChangedAPIResponseBO Diff(OpenApiResponses left, OpenApiResponses right, DiffContextBO context)
        {
            var responseMapKeyDiff = MapKeyDiff<string, OpenApiResponse>.Diff(left, right);
            var sharedResponseCodes = responseMapKeyDiff.SharedKey;
            var responses = new Dictionary<string, ChangedResponseBO>();
            foreach (var responseCode in sharedResponseCodes)
            {
                var diff = _openApiDiff
                    .ResponseDiff
                    .Diff(left[responseCode], right[responseCode], context);

                if(diff!= null)
                    responses.Add(responseCode, diff);
            }

            var changedApiResponse =
                new ChangedAPIResponseBO(left, right, context)
                {
                    Increased = responseMapKeyDiff.Increased,
                    Missing = responseMapKeyDiff.Missing,
                    Changed = responses,
                    Extensions = _openApiDiff
                        .ExtensionsDiff
                        .Diff(left.Extensions, right.Extensions, context)
                };

            return ChangedUtils.IsChanged(changedApiResponse);
        }
    }
}

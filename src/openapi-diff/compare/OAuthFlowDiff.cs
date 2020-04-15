using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.utils;

namespace openapi_diff.compare
{
    public class OAuthFlowDiff
    {
        private readonly OpenApiDiff _openApiDiff;
        public OAuthFlowDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
        }

        public ChangedOAuthFlowBO Diff(OpenApiOAuthFlow left, OpenApiOAuthFlow right)
        {
            var changedOAuthFlow = new ChangedOAuthFlowBO(left, right);
            if (left != null && right != null)
            {
                changedOAuthFlow
                    .IsAuthorizationUrl = left.AuthorizationUrl != right.AuthorizationUrl;
                changedOAuthFlow.IsTokenUrl = left.TokenUrl != right.TokenUrl;
                changedOAuthFlow.IsRefreshUrl = left.RefreshUrl != right.RefreshUrl;
            }

            changedOAuthFlow.Extensions = _openApiDiff
                .ExtensionsDiff
                .Diff(left?.Extensions, right?.Extensions);

            return ChangedUtils.IsChanged(changedOAuthFlow);
        }
    }
}

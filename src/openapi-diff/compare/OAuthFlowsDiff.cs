using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.utils;

namespace openapi_diff.compare
{
    public class OAuthFlowsDiff
    {
        private readonly OpenApiDiff _openApiDiff;
        public OAuthFlowsDiff(OpenApiDiff openApiDiff)
        {
            this._openApiDiff = openApiDiff;
        }

        public ChangedOAuthFlowsBO Diff(OpenApiOAuthFlows left, OpenApiOAuthFlows right)
        {
            var changedOAuthFlows = new ChangedOAuthFlowsBO(left, right);
            if (left != null && right != null)
            {
                changedOAuthFlows.ImplicitOAuthFlow = _openApiDiff
                    .OAuthFlowDiff
                    .Diff(left.Implicit, right.Implicit);
                changedOAuthFlows.PasswordOAuthFlow = _openApiDiff
                    .OAuthFlowDiff
                    .Diff(left.Password, right.Password);
                changedOAuthFlows.ClientCredentialOAuthFlow = _openApiDiff
                    .OAuthFlowDiff
                    .Diff(left.ClientCredentials, right.ClientCredentials);
                changedOAuthFlows.AuthorizationCodeOAuthFlow = _openApiDiff
                    .OAuthFlowDiff
                    .Diff(left.AuthorizationCode, right.AuthorizationCode);
            }

            changedOAuthFlows.Extensions = _openApiDiff
                .ExtensionsDiff
                .diff(left?.Extensions, right?.Extensions);

            return ChangedUtils.IsChanged(changedOAuthFlows);
        }
    }
}

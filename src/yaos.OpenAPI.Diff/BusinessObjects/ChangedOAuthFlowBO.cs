using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedOAuthFlowBO : ComposedChangedBO
    {
        private readonly OpenApiOAuthFlow _oldOAuthFlow;
        private readonly OpenApiOAuthFlow _newOAuthFlow;

        public bool IsAuthorizationUrl { get; set; }
        public bool IsTokenUrl { get; set; }
        public bool IsRefreshUrl { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedOAuthFlowBO(OpenApiOAuthFlow oldOAuthFlow, OpenApiOAuthFlow newOAuthFlow)
        {
            _oldOAuthFlow = oldOAuthFlow;
            _newOAuthFlow = newOAuthFlow;
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Extensions };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (IsAuthorizationUrl || IsTokenUrl || IsRefreshUrl)
            {
                return new DiffResultBO(DiffResultEnum.Incompatible);
            }
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }
    }
}

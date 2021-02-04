using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedOAuthFlowsBO : ComposedChangedBO
    {
        private readonly OpenApiOAuthFlows _newOAuthFlows;

        private readonly OpenApiOAuthFlows _oldOAuthFlows;

        public ChangedOAuthFlowsBO(OpenApiOAuthFlows oldOAuthFlows, OpenApiOAuthFlows newOAuthFlows)
        {
            _oldOAuthFlows = oldOAuthFlows;
            _newOAuthFlows = newOAuthFlows;
        }

        public ChangedOAuthFlowBO ImplicitOAuthFlow { get; set; }
        public ChangedOAuthFlowBO PasswordOAuthFlow { get; set; }
        public ChangedOAuthFlowBO ClientCredentialOAuthFlow { get; set; }
        public ChangedOAuthFlowBO AuthorizationCodeOAuthFlow { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.AuthFlow;
        }

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>
                {
                    ("Implicit", ImplicitOAuthFlow),
                    ("Password", PasswordOAuthFlow),
                    ("ClientCredential", ClientCredentialOAuthFlow),
                    ("AuthorizationCode", AuthorizationCodeOAuthFlow),
                    (null, Extensions)
                }
                .Where(x => x.Change != null).ToList();
        }

        public override DiffResultBO IsCoreChanged()
        {
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            return new List<ChangedInfoBO>();
        }
    }
}
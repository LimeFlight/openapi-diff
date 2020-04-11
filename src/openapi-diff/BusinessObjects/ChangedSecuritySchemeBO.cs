using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;

namespace openapi_diff.BusinessObjects
{
    public class ChangedSecuritySchemeBO : ComposedChangedBO
    {
        private readonly OpenApiSecurityScheme _oldSecurityScheme;
        private readonly OpenApiSecurityScheme _newSecurityScheme;

        public bool ChangedType { get; set; }
        public bool ChangedIn { get; set; }
        public bool ChangedScheme { get; set; }
        public bool ChangedBearerFormat { get; set; }
        public bool ChangedOpenIdConnectUrl { get; set; }
        public ChangedSecuritySchemeScopesBO ChangedScopes { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedOAuthFlowsBO OAuthFlows { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Description, OAuthFlows, Extensions };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (!ChangedType
                && !ChangedIn
                && !ChangedScheme
                && !ChangedBearerFormat
                && !ChangedOpenIdConnectUrl
                && (ChangedScopes == null || ChangedScopes.IsUnchanged()))
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (!ChangedType
                && !ChangedIn
                && !ChangedScheme
                && !ChangedBearerFormat
                && !ChangedOpenIdConnectUrl
                && (ChangedScopes == null || ChangedScopes.Increased.IsNullOrEmpty()))
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

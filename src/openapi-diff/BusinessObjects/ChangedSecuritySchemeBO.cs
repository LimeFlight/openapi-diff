using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;

namespace openapi_diff.BusinessObjects
{
    public class ChangedSecuritySchemeBO : ComposedChangedBO
    {
        public OpenApiSecurityScheme OldSecurityScheme { get; }
        public OpenApiSecurityScheme NewSecurityScheme { get; }

        public bool IsChangedType { get; set; }
        public bool IsChangedIn { get; set; }
        public bool IsChangedScheme { get; set; }
        public bool IsChangedBearerFormat { get; set; }
        public bool IsChangedOpenIdConnectUrl { get; set; }
        public ChangedSecuritySchemeScopesBO ChangedScopes { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedOAuthFlowsBO OAuthFlows { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedSecuritySchemeBO(OpenApiSecurityScheme oldSecurityScheme, OpenApiSecurityScheme newSecurityScheme)
        {
            OldSecurityScheme = oldSecurityScheme;
            NewSecurityScheme = newSecurityScheme;
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Description, OAuthFlows, Extensions };
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (!IsChangedType
                && !IsChangedIn
                && !IsChangedScheme
                && !IsChangedBearerFormat
                && !IsChangedOpenIdConnectUrl
                && (ChangedScopes == null || ChangedScopes.IsUnchanged()))
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (!IsChangedType
                && !IsChangedIn
                && !IsChangedScheme
                && !IsChangedBearerFormat
                && !IsChangedOpenIdConnectUrl
                && (ChangedScopes == null || ChangedScopes.Increased.IsNullOrEmpty()))
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

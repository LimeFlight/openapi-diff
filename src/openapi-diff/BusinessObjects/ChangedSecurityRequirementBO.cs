using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public class ChangedSecurityRequirementBO : ComposedChangedBO
    {
        private readonly OpenApiSecurityRequirement _oldSecurityRequirement;
        private readonly OpenApiSecurityRequirement _newSecurityRequirement;

        public OpenApiSecurityRequirement Missing { get; set; }
        public OpenApiSecurityRequirement Increased { get; set; }
        public List<ChangedSecuritySchemeBO> Changed { get; set; }

        public ChangedSecurityRequirementBO(OpenApiSecurityRequirement newSecurityRequirement, OpenApiSecurityRequirement oldSecurityRequirement)
        {
            _newSecurityRequirement = newSecurityRequirement;
            _oldSecurityRequirement = oldSecurityRequirement;
            Changed = new List<ChangedSecuritySchemeBO>();
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(Changed);
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Increased == null && Missing == null)
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (Increased == null)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

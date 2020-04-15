using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedSecurityRequirementsBO : ComposedChangedBO
    {
        private readonly IList<OpenApiSecurityRequirement> _oldSecurityRequirements;
        private readonly IList<OpenApiSecurityRequirement> _newSecurityRequirements;

        public List<OpenApiSecurityRequirement> Missing { get; set; }
        public List<OpenApiSecurityRequirement> Increased { get; set; }
        public List<ChangedSecurityRequirementBO> Changed { get; set; }

        public ChangedSecurityRequirementsBO(IList<OpenApiSecurityRequirement> oldSecurityRequirements, IList<OpenApiSecurityRequirement> newSecurityRequirements)
        {
            _oldSecurityRequirements = oldSecurityRequirements;
            _newSecurityRequirements = newSecurityRequirements;
            Missing = new List<OpenApiSecurityRequirement>();
            Increased = new List<OpenApiSecurityRequirement>();
            Changed = new List<ChangedSecurityRequirementBO>();
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(Changed);
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Missing.IsNullOrEmpty() && Increased.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

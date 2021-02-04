using System.Collections.Generic;
using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedSecuritySchemeScopesBO : ChangedListBO<string>
    {
        public ChangedSecuritySchemeScopesBO(List<string> oldValue, List<string> newValue) : base(oldValue, newValue,
            null)
        {
        }

        protected override ChangedElementTypeEnum GetElementType() => ChangedElementTypeEnum.SecuritySchemeScope;

        public override DiffResultBO IsItemsChanged()
        {
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
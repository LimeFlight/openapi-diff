using System.Collections.Generic;
using yaos.OpenAPI.Diff.Enums;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedSecuritySchemeScopesBO : ChangedListBO<string>
    {
        public ChangedSecuritySchemeScopesBO(List<string> oldValue, List<string> newValue) : base(oldValue, newValue, null)
        {
        }

        public override DiffResultBO IsItemsChanged()
        {
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

using openapi_diff.DTOs;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
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

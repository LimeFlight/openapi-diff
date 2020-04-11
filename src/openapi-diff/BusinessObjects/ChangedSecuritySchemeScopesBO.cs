using openapi_diff.DTOs;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedSecuritySchemeScopesBO : ChangedListBO<string>
    {
        public ChangedSecuritySchemeScopesBO(List<string> oldValue, List<string> newValue, DiffContextBO context) : base(oldValue, newValue, context)
        {
        }

        public override DiffResultBO IsItemsChanged()
        {
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

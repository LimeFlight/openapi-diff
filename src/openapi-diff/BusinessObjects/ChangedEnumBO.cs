using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedEnumBO : ChangedListBO<string>
    {
        public ChangedEnumBO(IList<string> oldValue, IList<string> newValue, DiffContextBO context) : base(oldValue, newValue, context)
        {
        }

        public override DiffResultBO IsItemsChanged()
        {
            if (Context.IsRequest && Missing.IsNullOrEmpty()
                || Context.IsResponse && Increased.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedRequiredBO : ChangedListBO<string>
    {
        public ChangedRequiredBO(List<string> oldValue, List<string> newValue, DiffContextBO context) : base(oldValue, newValue, context)
        {
        }

        public override DiffResultBO IsItemsChanged()
        {
            if (Context.IsRequest && Increased.IsNullOrEmpty()
                || Context.IsResponse && Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}

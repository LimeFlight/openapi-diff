using System.Collections.Generic;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
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

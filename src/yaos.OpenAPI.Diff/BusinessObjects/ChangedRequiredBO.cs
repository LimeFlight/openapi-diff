using System.Collections.Generic;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedRequiredBO : ChangedListBO<string>
    {
        protected override ChangedElementTypeEnum GetElementType() => ChangedElementTypeEnum.Required;

        public ChangedRequiredBO(IList<string> oldValue, IList<string> newValue, DiffContextBO context) : base(oldValue, newValue, context)
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

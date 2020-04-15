using System.Collections.Generic;
using System.Linq;
using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public abstract class ComposedChangedBO : ChangedBO
    {
        public abstract List<ChangedBO> GetChangedElements();

        public abstract DiffResultBO IsCoreChanged();

        public override DiffResultBO IsChanged()
        {
            var elementsResultMax = GetChangedElements()
                .Where(x => x != null)
                .Select(x => (int)x.IsChanged().DiffResult)
                .DefaultIfEmpty(0)
                .Max();

            var elementsResult = new DiffResultBO((DiffResultEnum)elementsResultMax);
            
            return IsCoreChanged().DiffResult > elementsResult.DiffResult ? IsCoreChanged() : elementsResult;
        }
    }
}

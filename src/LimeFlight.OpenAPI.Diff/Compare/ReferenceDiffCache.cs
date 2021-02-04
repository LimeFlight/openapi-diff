using System.Collections.Generic;
using LimeFlight.OpenAPI.Diff.BusinessObjects;

namespace LimeFlight.OpenAPI.Diff.Compare
{
    public abstract class ReferenceDiffCache<TC, TD>
        where TD : class
    {
        protected ReferenceDiffCache()
        {
            RefDiffMap = new Dictionary<CacheKey, TD>();
        }

        public Dictionary<CacheKey, TD> RefDiffMap { get; set; }

        protected abstract TD ComputeDiff(TC left, TC right, DiffContextBO context);

        public TD CachedDiff(
            TC left,
            TC right,
            string leftRef,
            string rightRef,
            DiffContextBO context)
        {
            var areBothRefParameters = leftRef != null && rightRef != null;
            if (areBothRefParameters)
            {
                var key = new CacheKey(leftRef, rightRef, context);
                if (RefDiffMap.TryGetValue(key, out var changedFromRef))
                    return changedFromRef;

                RefDiffMap.Add(key, null);
                var changed = ComputeDiff(left, right, context);
                RefDiffMap[key] = changed;
                return changed;
            }

            return ComputeDiff(left, right, context);
        }
    }
}
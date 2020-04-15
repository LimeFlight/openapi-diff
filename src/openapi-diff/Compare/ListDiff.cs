using openapi_diff.BusinessObjects;
using openapi_diff.Extensions;

namespace openapi_diff.Compare
{
    public static class ListDiff
    {
        public static T1 Diff<T1, T2>(T1 instance)
            where T1 : ChangedListBO<T2>
        {
            if (instance.OldValue.IsNullOrEmpty() && instance.NewValue.IsNullOrEmpty())
            {
                return instance;
            }
            if (instance.OldValue.IsNullOrEmpty())
            {
                instance.Increased = instance.NewValue;
                return instance;
            }
            if (instance.NewValue.IsNullOrEmpty())
            {
                instance.Missing = instance.OldValue;
                return instance;
            }
            instance.Increased.AddRange(instance.NewValue);
            foreach (var leftItem in instance.OldValue)
            {
                if (instance.NewValue.Contains(leftItem))
                {
                    instance.Increased.Remove(leftItem);
                    instance.Shared.Add(leftItem);
                }
                else
                {
                    instance.Missing.Add(leftItem);
                }
            }
            return instance;
        }
    }
}

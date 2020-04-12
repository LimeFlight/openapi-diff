using openapi_diff.BusinessObjects;

namespace openapi_diff.Compare
{
    public static class ListDiff
    {
        public static T1 diff<T1, T2>(T1 instance)
            where T1 : ChangedListBO<T2>
        {
            if (instance.OldValue == null && instance.NewValue == null)
            {
                return instance;
            }
            if (instance.OldValue == null)
            {
                instance.Increased = instance.NewValue;
                return instance;
            }
            if (instance.NewValue == null)
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

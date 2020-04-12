using System.Collections.Generic;

namespace openapi_diff.Compare
{
    public class MapKeyDiff<T1, T2>
    {
        public Dictionary<T1, T2> Increased { get; set; }
        public Dictionary<T1, T2> Missing { get; set; }
        public List<T1> SharedKey { get; set; }

        private MapKeyDiff()
        {
            SharedKey = new List<T1>();
            Increased = new Dictionary<T1, T2>();
            Missing = new Dictionary<T1, T2>();
        }

        public static MapKeyDiff<T1, T2> Diff(Dictionary<T1, T2> mapLeft, Dictionary<T1, T2> mapRight)
        {
            var instance = new MapKeyDiff<T1, T2>();
            if (null == mapLeft && null == mapRight) return instance;
            if (null == mapLeft)
            {
                instance.Increased = mapRight;
                return instance;
            }
            if (null == mapRight)
            {
                instance.Missing = mapLeft;
                return instance;
            }
            instance.Increased = new Dictionary<T1, T2>(mapRight);
            instance.Missing = new Dictionary<T1, T2>();
            foreach (var entry in mapLeft)
            {
                var leftKey = entry.Key;
                var leftValue = entry.Value;
                if (mapRight.ContainsKey(leftKey))
                {
                    instance.Increased.Remove(leftKey);
                    instance.SharedKey.Add(leftKey);
                }
                else
                {
                    instance.Missing.Add(leftKey, leftValue);
                }
            }
            return instance;
        }
    }
}

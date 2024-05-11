namespace BeireMKit.Domain.Extensions
{
    public static class DictionaryExtension
    {
        public static Dictionary<Key, Value> MergeInPlace<Key, Value>(this Dictionary<Key, Value> left, Dictionary<Key, Value> right)
        {
            if (left == null)
                return right ?? new Dictionary<Key, Value>();

            if (right != null)
            {
                foreach (var kvp in right)
                {
                    if (!left.ContainsKey(kvp.Key))
                    {
                        left[kvp.Key] = kvp.Value;
                    }
                }
            }

            return left;
        }
    }
}

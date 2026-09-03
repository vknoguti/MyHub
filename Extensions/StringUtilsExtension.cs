using System.ComponentModel;

namespace MyHub.Extensions
{
    public static class StringUtilsExtension
    {
        public static TKey? ConvertTo<TKey>(this string? key) 
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            if (typeof(TKey) == typeof(string))
            {
                return (TKey)(object)key;
            }
            if (typeof(TKey) == typeof(Guid))
            {
                var parsed = Guid.TryParse(key, out var keyParsed);
                return parsed ? (TKey)(object)keyParsed : default;
            }
            if(typeof(TKey) == typeof(int))
            {
                var parsed = int.TryParse(key, out var keyParsed);
                return parsed ? (TKey)(object)keyParsed : default;
            }
            if (typeof(TKey) == typeof(long))
            {
                var parsed = long.TryParse(key, out var keyParsed);
                return parsed ? (TKey)(object)keyParsed : default;
            }
            return default;
        }
    }
}

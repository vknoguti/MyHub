using System.ComponentModel;

namespace MyHub.Extensions
{
    public static class StringUtilsExtension
    {
        public static TKey? ConvertTo<TKey>(this string? key) 
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(TKey);
            }

            var converter = TypeDescriptor.GetConverter(typeof(TKey));
            if(converter != null && converter.CanConvertTo(typeof(TKey)))
            {
                return (TKey)converter.ConvertFromInvariantString(key)!;
            }

            return (TKey)Convert.ChangeType(key, typeof(TKey));
        }
    }
}

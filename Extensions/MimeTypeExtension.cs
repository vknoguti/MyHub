using Microsoft.AspNetCore.StaticFiles;

namespace MyHub.Extensions
{
    public static class MimeTypeExtension
    {
        public static string? GetExtensionFromMimeType(string mimeType)
        {
            var provider = new FileExtensionContentTypeProvider();
            foreach (var map in provider.Mappings)
            {
                if(string.Equals(map.Value, mimeType, StringComparison.OrdinalIgnoreCase))
                {
                    return map.Key;
                }
            }
            return null;
        }
    }
}

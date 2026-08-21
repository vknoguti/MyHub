using MyHub.Enums;
using System.ComponentModel;
using System.Reflection;
namespace MyHub.Extensions
{
    public static class AuthenticationStatusExtension
    {
        public static string? GetDescriptionMessage(this Enum status)
        {
            FieldInfo? field = status.GetType().GetField(status.ToString());
            if (field is null) return null;

            var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (attribute is null) return null;

            return attribute.Description;
        }
    }
}

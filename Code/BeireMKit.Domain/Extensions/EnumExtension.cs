using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BeireMKit.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum value)
        {
            DisplayAttribute attr = GetDisplayAttribute(value);
            return attr?.Name ?? value.ToString();
        }

        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Type {typeof(T)} is not an enumeration.");
            }

            return Enum.TryParse<T>(str, true, out T val) ? val : default;
        }
        
        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Type {typeof(T)} is not an enumeration.");
            }

            return Enum.IsDefined(enumType, intValue)
                ? (T)Enum.ToObject(enumType, intValue)
                : default;
        }

        public static string GetGroupName(this Enum value)
        {
            DisplayAttribute attr = GetDisplayAttribute(value);
            return attr?.GroupName ?? value.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"Type {type} is not an enumeration.");
            }

            FieldInfo field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}

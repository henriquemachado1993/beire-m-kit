using Newtonsoft.Json;
using System.Text;

namespace BeireMKit.Domain.Extensions
{
    public static class Base64Extension
    {
        public static string EncodeBase64(this string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeBase64(this string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return base64EncodedData;

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string EncodeBase64<T>(this T obj)
        {
            if (obj == null)
                return string.Empty;

            string plainText = JsonConvert.SerializeObject(obj);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static T? DecodeBase64<T>(this string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return default;

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            string plainText = Encoding.UTF8.GetString(base64EncodedBytes);
            return JsonConvert.DeserializeObject<T>(plainText);
        }
    }
}

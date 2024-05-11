using Newtonsoft.Json;

namespace BeireMKit.Domain.Extensions
{
    public static class HttpContentExtension
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "HttpContent cannot be null.");
            }

            string json = await content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(json))
            {
                throw new InvalidOperationException("Empty or null JSON content.");
            }

            try
            {
                T value = JsonConvert.DeserializeObject<T>(json);
                return value;
            }
            catch (JsonException ex)
            {
                throw new JsonException("Error deserializing JSON content.", ex);
            }
        }
    }
}

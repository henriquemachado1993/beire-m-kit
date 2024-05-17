namespace BeireMKit.Authetication.Models
{
    public class GoogleSettings
    {
        public GoogleSettings() { }

        public GoogleSettings(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}

namespace BeireMKit.Authetication.Models
{
    public class JwtSettings
    {
        public JwtSettings() { }

        public JwtSettings(string secretKey, string issuer, string audience) {
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
        }

        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}

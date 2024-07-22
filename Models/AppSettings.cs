namespace CustomersApi.Model
{
    public class AppSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpirationInMinutes { get; set; } = 0;
    }
}
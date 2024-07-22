namespace CustomersApi.Model
{
    public class AppSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required int TokenExpirationInMinutes { get; set; }
    }
}
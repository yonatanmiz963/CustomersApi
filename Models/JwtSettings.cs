public class JwtSettings
{
    public int TokenExpirationInMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}

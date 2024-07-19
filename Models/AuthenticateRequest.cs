using System.ComponentModel;

namespace CustomersApi.Models
{
    public class AuthenticateRequest
    {
        [DefaultValue("yonatanmiz963@gmail.com")]
        public required string Email { get; set; }

        [DefaultValue("admin")]
        public required string Password { get; set; }

        [DefaultValue("Yonatan")]
        public required string FirstName { get; set; }

        [DefaultValue("Mizrahi")]
        public required string LastName { get; set; }
    }
}
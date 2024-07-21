using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomersApi.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        [DefaultValue("yonatanmiz963@gmail.com")]
        public required string Email { get; set; }


        [Required]
        [DefaultValue("admin")]
        [Length(5, 20)]
        public required string Password { get; set; }

        [Required]
        [DefaultValue("Yonatan")]
        public required string FirstName { get; set; }

        [Required]
        [DefaultValue("Mizrahi")]
        public required string LastName { get; set; }
    }
}
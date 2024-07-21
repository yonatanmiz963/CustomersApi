using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomersApi.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        [DefaultValue("yonatanmiz963@gmail.com")]
        public string? Email { get; set; }


        [Required]
        [DefaultValue("admin")]
        [Length(5, 20)]
        public string? Password { get; set; }

        [Required]
        [DefaultValue("Yonatan")]
        public string? FirstName { get; set; }

        [Required]
        [DefaultValue("Mizrahi")]
        public string? LastName { get; set; }
    }
}
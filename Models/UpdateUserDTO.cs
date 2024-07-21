
using System.ComponentModel.DataAnnotations;

namespace CustomersApi.Models
{
    public class UpdateUserDTO
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Bank account must be numeric")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Bank account must be exactly 7 digits long")]
        public string? BankAccount { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}
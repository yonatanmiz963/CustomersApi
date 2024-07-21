
using System.ComponentModel.DataAnnotations;

namespace CustomersApi.Models
{
    public class UpdateUserDTO
    {
        [Required]
        public int? Id { get; set; }
        public string? FirstName { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Bank account must be numeric")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Bank account must be exactly 7 digits long")]
        public string? BankAccount { get; set; }
        public string? LastName { get; set; }
    }
}
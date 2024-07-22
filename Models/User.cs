
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomersApi.Models;

public class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public required string FirstName { get; set; }

    public string? BankAccount { get; set; }

    public required string LastName { get; set; }

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
    [StringLength(7, MinimumLength = 7, ErrorMessage = "The PhoneNumber must be exactly 7 characters long.")]
    [RegularExpression("^[0-9]{7}$", ErrorMessage = "The PhoneNumber must be exactly 7 digits.")]
    public string? PhoneNumber { get; set; }

    public required string HashPassword { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomersApi.Models;

public class Customer
{
    public int Id { get; set; }
    [Required]
    public required string Email {get; set;}
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string? PhoneNumber { get; set; }
    public required string HashPassword { get; set; }
}
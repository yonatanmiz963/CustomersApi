

namespace CustomersApi.Models;

public class UserDTO
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? BankAccount { get; set; }
    public string? LastName { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string? PhoneNumber { get; set; }
}
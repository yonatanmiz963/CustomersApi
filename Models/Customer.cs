
namespace CustomersApi.Models;

public class Customer
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string? PhoneNumber { get; set; }
}
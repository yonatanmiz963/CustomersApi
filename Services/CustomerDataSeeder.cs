using CustomersApi.Models;

public class CustomerDataSeeder
{
    private readonly CustomerContext _db;

    public CustomerDataSeeder(CustomerContext db)
    {
        _db = db;
    }

    public void SeedData()
    {
        _db.CustomerItems.AddRange(GetTestData());
        _db.SaveChanges();
    }

    private List<Customer> GetTestData()
    {
        return new List<Customer>()
        {
            new Customer {  Name = "Ada", LastName = "Lovelace", PhoneNumber = "123456789"},
            new Customer {  Name = "Ada", LastName = "Lovelace", PhoneNumber = "123456789"}, 
            new Customer {  Name = "Ada", LastName = "Lovelace", PhoneNumber = "123456789"},
            new Customer {  Name = "Ada", LastName = "Lovelace", PhoneNumber = "123456789"},
        };
    }
}

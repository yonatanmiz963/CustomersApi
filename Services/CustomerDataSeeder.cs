using CustomersApi.Models;

public class CustomerDataSeeder
{
    private readonly CustomerContext _db;
    private readonly IPasswordUtilityService _passwordUtilityService;


    public CustomerDataSeeder(CustomerContext db, IPasswordUtilityService passwordUtilityService)
    {
        _db = db;
        _passwordUtilityService = passwordUtilityService;
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
            new Customer {  FirstName = "Yonatan", Email = "yonatanmiz963@gmail.com", LastName = "Mizrahi", PhoneNumber = "123456789", HashPassword = _passwordUtilityService.HashPassword("admin")},
        };
    }
}

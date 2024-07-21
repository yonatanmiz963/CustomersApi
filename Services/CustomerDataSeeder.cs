using CustomersApi.Models;

public class CustomerDataSeeder
{
    private readonly UsersContext _db;
    private readonly IPasswordUtilityService _passwordUtilityService;


    public CustomerDataSeeder(UsersContext db, IPasswordUtilityService passwordUtilityService)
    {
        _db = db;
        _passwordUtilityService = passwordUtilityService;
    }

    public void SeedData()
    {
        _db.UserItems.AddRange(GetTestData());
        _db.SaveChanges();
    }

    private List<User> GetTestData()
    {
        return new List<User>()
        {
            new User {  BankAccount = "753724774", FirstName = "Yonatan", Email = "yonatanmiz963@gmail.com", LastName = "Mizrahi", PhoneNumber = "123456789", HashPassword = _passwordUtilityService.HashPassword("admin")},
        };
    }
}

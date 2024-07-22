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
            new User {  BankAccount = "753724743", FirstName = "Yonatan", Email = "yonatanmiz963@gmail.com", LastName = "Mizrahi", PhoneNumber = "11111111", HashPassword = _passwordUtilityService.HashPassword("admin")},
            new User {  BankAccount = "534534543", FirstName = "Hadas", Email = "hadas@gmail.com", LastName = "Bizrahi", PhoneNumber = "222222222", HashPassword = _passwordUtilityService.HashPassword("admin")},
            new User {  BankAccount = "345345344", FirstName = "Daniel", Email = "daniel@gmail.com", LastName = "Aizrahi", PhoneNumber = "3333333333", HashPassword = _passwordUtilityService.HashPassword("admin")},
            new User {  BankAccount = "345435344", FirstName = "David", Email = "david@gmail.com", LastName = "Cizrahi", PhoneNumber = "4444444444", HashPassword = _passwordUtilityService.HashPassword("admin")},
        };
    }
}

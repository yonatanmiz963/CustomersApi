using Newtonsoft.Json;
using CustomersApi.Models;

public class UserDataInitializer
{
    private readonly string _filePath = "users.json";
    private readonly IPasswordUtilityService _passwordUtilityService;

    public UserDataInitializer(IPasswordUtilityService passwordUtilityService)
    {
        _passwordUtilityService = passwordUtilityService;
    }

    public void Initialize()
    {
        if (File.Exists(_filePath))
        {
            // If the file already exists, no need to initialize
            return;
        }

        var users = new List<User>
        {
            new User { Id = 1, BankAccount = "5555555", FirstName = "Yonatan", Email = "yonatanmiz963@gmail.com", LastName = "Mizrahi", PhoneNumber = "0543971495", HashPassword = _passwordUtilityService.HashPassword("admin") }
        };
        for (int i = 2; i <= 100; i++)
        {
            users.Add(new User
            {
                Id = i,
                BankAccount = GenerateBankAccount(i), // Assuming a method to generate unique bank accounts
                FirstName = $"User{i}",
                Email = $"user{i}@example.com",
                LastName = $"LastName{i}",
                PhoneNumber = GeneratePhoneNumber(i), // Assuming a method to generate unique phone numbers
                HashPassword = _passwordUtilityService.HashPassword("admin")
            });
        }

        var jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(_filePath, jsonData);
    }

    // Example methods for generating bank accounts and phone numbers
    private string GenerateBankAccount(int seed)
    {
        return (123456789 + seed * 123).ToString();
    }

    private string GeneratePhoneNumber(int seed)
    {
        return (11111111 + seed * 111111).ToString();
    }


}

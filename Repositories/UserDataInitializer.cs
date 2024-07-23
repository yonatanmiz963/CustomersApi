using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomersApi.Models;
using Microsoft.Extensions.DependencyInjection;

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
            new() { Id = 1, BankAccount = "753724743", FirstName = "Yonatan", Email = "yonatanmiz963@gmail.com", LastName = "Mizrahi", PhoneNumber = "11111111", HashPassword = _passwordUtilityService.HashPassword("admin")},
            new() { Id = 2, BankAccount = "534534543", FirstName = "Hadas", Email = "hadas@gmail.com", LastName = "Bizrahi", PhoneNumber = "222222222", HashPassword = _passwordUtilityService.HashPassword("admin")},
            new() { Id = 3, BankAccount = "345345344", FirstName = "Daniel", Email = "daniel@gmail.com", LastName = "Aizrahi", PhoneNumber = "3333333333", HashPassword = _passwordUtilityService.HashPassword("admin")},
            new() { Id = 4, BankAccount = "345435344", FirstName = "David", Email = "david@gmail.com", LastName = "Cizrahi", PhoneNumber = "4444444444", HashPassword = _passwordUtilityService.HashPassword("admin")},

        };

        var jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(_filePath, jsonData);
    }
}

using Newtonsoft.Json;
using CustomersApi.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<UserDTO?> GetByIdAsync(int id);
    Task<UserDTO?> UpdateAsync(UpdateUserDTO user);
    Task<bool> DeleteAsync(int id);
    Task<User?> FindUserByCredentialsAsync(string firstName, string lastName, string email, string password);

}

public class UserRepository : IUserRepository
{
    private readonly string _filePath = "users.json";
    private List<User> _users;
    private readonly ILogger<UserRepository> _logger;

    private readonly IPasswordUtilityService _passwordUtilityService;

    public UserRepository(ILogger<UserRepository> logger, IPasswordUtilityService passwordUtilityService)
    {
        _logger = logger;
        _passwordUtilityService = passwordUtilityService;
        _users = LoadUsers();
    }

    public async Task<User?> FindUserByCredentialsAsync(string firstName, string lastName, string email, string password)
    {
        return await Task.FromResult(_users.SingleOrDefault(x => x.FirstName == firstName && x.LastName == lastName && x.Email == email && _passwordUtilityService.VerifyPassword(password, x.HashPassword)));
    }

    private List<User> LoadUsers()
    {
        if (File.Exists(_filePath))
        {
            var jsonData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
        }
        return new List<User>();
    }

    private void SaveUsers()
    {
        var jsonData = JsonConvert.SerializeObject(_users, Formatting.Indented);
        File.WriteAllText(_filePath, jsonData);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all users from the repository.");
        return await Task.FromResult(_users);
    }

    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching user with ID {UserId} from the repository.", id);
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            return await Task.FromResult(new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BankAccount = user.BankAccount,
                Date = user.Date,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            });
        }
        return null; // User not found
    }

public async Task<UserDTO?> UpdateAsync(UpdateUserDTO userObj)
{
    _logger.LogInformation("Updating user with ID {UserId} in the repository.", userObj.Id);
    var existingUser = _users.FirstOrDefault(u => u.Id == userObj.Id);
    if (existingUser != null)
    {
        existingUser.FirstName = userObj.FirstName;
        existingUser.LastName = userObj.LastName;
        existingUser.BankAccount = userObj.BankAccount;
        SaveUsers(); // Assume SaveUsers saves changes to an in-memory collection
        return await Task.FromResult(new UserDTO
        {
            Id = existingUser.Id,
            FirstName = existingUser.FirstName,
            LastName = existingUser.LastName,
            BankAccount = existingUser.BankAccount,
            Date = existingUser.Date,
            Email = existingUser.Email,
            PhoneNumber = existingUser.PhoneNumber,
        });
    }
    return null; // User not found
}

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting user with ID {UserId} from the repository.", id);
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            await Task.Run(() => SaveUsers());
            return true; // Deleted successfully
        }
        return false; // User not found
    }
}

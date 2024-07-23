using CustomersApi.Model;
using CustomersApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync();
    Task<UserDTO?> GetByIdAsync(int id);
    Task<UserDTO?> UpdateAsync(UpdateUserDTO updatedUserObj);
    Task<bool> DeleteAsync(int id);
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _config;


    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IOptions<JwtSettings> jwtSettings, IConfiguration config)
    {
        _userRepository = userRepository;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
        _config = config;
    }

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
    {
        var user = await _userRepository.FindUserByCredentialsAsync(model.FirstName, model.LastName, model.Email, model.Password);
    
        // return null if User not found
        if (user == null) return null;
    
        // authentication successful so generate jwt token
        var token = await GenerateJwtTokenAsync(user);
    
        return new AuthenticateResponse(user, token, DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes));
    }


    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all users.");
        return (await _userRepository.GetAllAsync()).Select(user => new UserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BankAccount = user.BankAccount,
            Date = user.Date,
            PhoneNumber = user.PhoneNumber
        }).ToList();
    }

    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching user with ID {UserId}.", id);
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        return user;
    }


    public async Task<UserDTO?> UpdateAsync(UpdateUserDTO updatedUserObj)
    {
        _logger.LogInformation("Updating user with ID {UserId}.", updatedUserObj.Id);
        return await _userRepository.UpdateAsync(updatedUserObj);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting user with ID {UserId}.", id);
        return await _userRepository.DeleteAsync(id);
    }

private async Task<string> GenerateJwtTokenAsync(User user)
{
    // Simulate an asynchronous operation with Task.FromResult
    return await Task.Run(() =>
    {
        // Generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? string.Empty);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    });
}
}

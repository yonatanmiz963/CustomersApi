
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomersApi.Models;
using CustomersApi.Model;



namespace CustomersApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UsersContext _db;
        private readonly IPasswordUtilityService _passwordUtilityService;
        private readonly IConfiguration _config;


        public UserService(IOptions<AppSettings> appSettings, UsersContext db, IPasswordUtilityService passwordUtility, IConfiguration config)
        {
            _appSettings = appSettings.Value;
            _db = db;
            _passwordUtilityService = passwordUtility;
            _config = config;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var User = await _db.UserItems.SingleOrDefaultAsync(x => x.FirstName == model.FirstName && _passwordUtilityService.VerifyPassword(model.Password, x.HashPassword) && x.LastName == model.LastName && x.Email == model.Email);

            // return null if User not found
            if (User == null) return null;

            // authentication successful so generate jwt token
            var token = await generateJwtToken(User);

            return new AuthenticateResponse(User, token, DateTime.UtcNow.AddMinutes(_appSettings.TokenExpirationInMinutes));
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _db.UserItems.ToListAsync();
            var userDTOs = users.Select(user => new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BankAccount = user.BankAccount,
                Date = user.Date,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            }).ToList();

            return userDTOs;
        }

        public async Task<UserDTO?> GetById(int id)
        {
            var user = await _db.UserItems.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BankAccount = user.BankAccount,
                    Date = user.Date,
                    Email = user.Email,
                };
            }
            return null;
        }
        public async Task<UserDTO?> UpdateUser(UpdateUserDTO userObj)
        {
            var user = await _db.UserItems.FindAsync(userObj.Id);
            if (user != null)
            {
                user.FirstName = userObj.FirstName;
                user.LastName = userObj.LastName;
                user.BankAccount = userObj.BankAccount;

                await _db.SaveChangesAsync();
                return new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BankAccount = user.BankAccount,
                    Date = user.Date,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };
            }
            return null;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _db.UserItems.FindAsync(userId);
            if (user != null)
            {
                _db.UserItems.Remove(user);
                await _db.SaveChangesAsync();
            }
        }

        private async Task<string> generateJwtToken(User User)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? string.Empty);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // Audience = _appSettings.Audience,
                    // Issuer = _appSettings.Issuer,
                    Subject = new ClaimsIdentity(new[] { new Claim("id", User.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddMinutes(_appSettings.TokenExpirationInMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
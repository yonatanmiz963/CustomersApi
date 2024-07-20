using CustomersApi.Model;
using CustomersApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<Customer>> GetAll();
    Task<Customer?> GetById(int id);
    Task<Customer?> AddAndUpdateUser(Customer userObj);
}

namespace CustomersApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly CustomerContext _db;
        private readonly IPasswordUtilityService _passwordUtilityService;

        public UserService(IOptions<AppSettings> appSettings, CustomerContext db, IPasswordUtilityService passwordUtility)
        {
            _appSettings = appSettings.Value;
            _db = db;
            _passwordUtilityService = passwordUtility;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var Customer = await _db.CustomerItems.SingleOrDefaultAsync(x => x.FirstName == model.FirstName && _passwordUtilityService.VerifyPassword(model.Password, x.HashPassword) && x.LastName == model.LastName && x.Email == model.Email);

            // return null if Customer not found
            if (Customer == null) return null;

            // authentication successful so generate jwt token
            var token = await generateJwtToken(Customer);

            return new AuthenticateResponse(Customer, token);
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _db.CustomerItems.ToListAsync();
        }

        public async Task<Customer?> GetById(int id)
        {
            return await _db.CustomerItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer?> AddAndUpdateUser(Customer userObj)
        {
            bool isSuccess = false;
            if (userObj.Id > 0)
            {
                var obj = await _db.CustomerItems.FirstOrDefaultAsync(c => c.Id == userObj.Id);
                if (obj != null)
                {
                    obj.FirstName = userObj.FirstName;
                    obj.LastName = userObj.LastName;
                    _db.CustomerItems.Update(obj);
                    isSuccess = await _db.SaveChangesAsync() > 0;
                }
            }
            else
            {
                await _db.CustomerItems.AddAsync(userObj);
                isSuccess = await _db.SaveChangesAsync() > 0;
            }

            return isSuccess ? userObj : null;
        }
        // helper methods
        private async Task<string> generateJwtToken(Customer Customer)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", Customer.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
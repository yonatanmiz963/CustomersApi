using CustomersApi.Model;
using CustomersApi.Models;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(int id);
    Task<User?> UpdateUser(User userObj);
    Task DeleteUser(int id);
}
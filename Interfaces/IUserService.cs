using CustomersApi.Model;
using CustomersApi.Models;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<UserDTO>> GetAll();
    Task<UserDTO?> GetById(int id);
    Task<UserDTO?> UpdateUser(UpdateUserDTO userObj);
    Task DeleteUser(int id);
}
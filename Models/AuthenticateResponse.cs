using CustomersApi.Models;

namespace CustomersApi.Model
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token, DateTime expirationDate)
        {
            Id = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ExpirationDate = expirationDate;
            Token = token;
        }
    }
}
public interface IPasswordUtilityService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
namespace BeireMKit.Authetication.Interfaces.Jwt
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}

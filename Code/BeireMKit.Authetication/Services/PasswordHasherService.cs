using BeireMKit.Authetication.Interfaces.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BeireMKit.Authetication.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}:{hashed}";
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2)
        {
            throw new FormatException("Unexpected hash format. Should be formatted as '{salt}:{hash}'.");
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        string hashedProvidedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: providedPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hash == hashedProvidedPassword;
    }
}


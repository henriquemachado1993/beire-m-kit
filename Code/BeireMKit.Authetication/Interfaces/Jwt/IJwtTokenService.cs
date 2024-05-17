using System.Security.Claims;

namespace BeireMKit.Authetication.Interfaces.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateJwtTokenIdentity(Claim[] claims, int expiresInMinute = 30);
        string GenerateJwtToken(Claim[] claims, int expiresInMinute = 30);
    }
}

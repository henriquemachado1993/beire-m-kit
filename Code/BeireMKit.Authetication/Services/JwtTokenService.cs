using BeireMKit.Authetication.Interfaces.Jwt;
using BeireMKit.Authetication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeireMKit.Authetication.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateJwtTokenIdentity(Claim[] claims, int expiresInMinute = 30)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiresInMinute),
            SigningCredentials = creds,
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateJwtToken(Claim[] claims, int expiresInMinute = 30)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinute),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
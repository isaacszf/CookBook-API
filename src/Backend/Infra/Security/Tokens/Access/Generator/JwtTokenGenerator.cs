using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Security.Tokens.Access.Generator;

public class JwtTokenGenerator : JwtTokenHelper, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signInKey;

    public JwtTokenGenerator(uint expirationTimeMinutes, string signInKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signInKey = signInKey;
    }
    
    public string Generate(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, userId.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                ConvertToSecurityKey(_signInKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
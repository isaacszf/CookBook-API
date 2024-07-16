using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Security.Tokens.Access.Validator;

public class JwtTokenValidator : JwtTokenHelper, IAccessTokenValidator
{
    private readonly string _signInKey;

    public JwtTokenValidator(string signInKey) => _signInKey = signInKey;
    
    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, // valida quem irá utilizar o token (deixo como false)
            ValidateIssuer = false, // valida quem foi o emissor do token (deixo como false)
            IssuerSigningKey = ConvertToSecurityKey(_signInKey),
            ClockSkew = new TimeSpan(0),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = tokenHandler.ValidateToken(token, validationParameters, out _);

        var userIdentifier = 
            claims.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userIdentifier);
    }
}
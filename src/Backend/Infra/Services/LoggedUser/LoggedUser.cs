using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Entities;
using Domain.Security.Tokens;
using Domain.Services.LoggedUser;
using Infra.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infra.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly CookBookDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(CookBookDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var userIdentifier = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        return await _dbContext.Users
            .AsNoTracking()
            .FirstAsync(u =>
                u.Active &&
                u.UserIdentifier == userIdentifier);
    }
}
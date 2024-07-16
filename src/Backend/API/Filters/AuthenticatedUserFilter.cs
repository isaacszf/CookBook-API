using Communication.Responses;
using Domain.Repositories.User;
using Domain.Security.Tokens;
using Exceptions;
using Exceptions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _readOnlyRepository;

    public AuthenticatedUserFilter(
        IAccessTokenValidator accessTokenValidator,
        IUserReadOnlyRepository readOnlyRepository)
    {
        _accessTokenValidator = accessTokenValidator;
        _readOnlyRepository = readOnlyRepository;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = GetTokenOnHeaders(context);
            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await _readOnlyRepository.ExistsActiveUserWithIdentifier(userIdentifier);
            if (!exist) throw new CookBookException(ResourceMessageException.USER_WITHOUT_PERMISSION);
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Token Expired")
            {
                TokenIsExpired = true
            });
        }
        catch (CookBookException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson(ResourceMessageException.USER_WITHOUT_PERMISSION));
        }
    }

    private static string GetTokenOnHeaders(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication)) 
            throw new CookBookException(ResourceMessageException.NO_TOKEN);
        
        return authentication.Replace("Bearer ", "").Trim();
    }
}
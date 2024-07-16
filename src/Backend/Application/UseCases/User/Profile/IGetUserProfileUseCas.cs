using Communication.Responses;
using Domain.Services.LoggedUser;

namespace Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUseCase(ILoggedUser loggedUser) => _loggedUser = loggedUser;
    
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();
        return new ResponseUserProfileJson
        {
            Name = user.Name,
            Email = user.Email
        };
    }
}
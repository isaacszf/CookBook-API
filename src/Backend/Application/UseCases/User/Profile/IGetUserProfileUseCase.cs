using Communication.Responses;

namespace Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
using Communication.Requests;

namespace Application.UseCases.User.Profile;

public interface IUpdateProfileUseCase
{
    public Task Execute(RequestUpdateUserJson req);
}
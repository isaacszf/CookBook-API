using Communication.Requests;

namespace Application.UseCases.User.ChangePassword;

public interface IChangeUserPasswordUseCase
{
    public Task Execute(RequestChangeUserPasswordJson req);
}
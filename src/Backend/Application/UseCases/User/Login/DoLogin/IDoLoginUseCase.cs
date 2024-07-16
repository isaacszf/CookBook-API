using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.User.Login.DoLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseLoginUserJson> Execute(RequestLoginUserJson req);
}
using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson req);
}
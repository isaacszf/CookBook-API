using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Recipe.Register;

public interface ICreateRecipeUseCase
{
    public Task<ResponseCreatedRecipeJson> Execute(RequestRecipeJson req);
}
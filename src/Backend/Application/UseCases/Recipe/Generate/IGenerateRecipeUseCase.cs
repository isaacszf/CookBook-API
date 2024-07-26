using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Recipe.Generate;

public interface IGenerateRecipeUseCase
{
    public Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson req);
}
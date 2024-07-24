using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    public Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson req);
}
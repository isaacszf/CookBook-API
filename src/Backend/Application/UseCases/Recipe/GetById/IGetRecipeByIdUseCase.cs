using Communication.Responses;

namespace Application.UseCases.Recipe.GetById;

public interface IGetRecipeByIdUseCase
{
    public Task<ResponseRecipeJson> Execute(long recipeId);
}
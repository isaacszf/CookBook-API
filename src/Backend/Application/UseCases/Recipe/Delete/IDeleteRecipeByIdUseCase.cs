namespace Application.UseCases.Recipe.Delete;

public interface IDeleteRecipeByIdUseCase
{
    public Task Execute(long recipeId);
}
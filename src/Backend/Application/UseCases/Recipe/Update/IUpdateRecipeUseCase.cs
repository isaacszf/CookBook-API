using Communication.Requests;

namespace Application.UseCases.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    public Task Execute(long id, RequestRecipeJson req);
}
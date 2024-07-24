using Domain.Dtos;

namespace Domain.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    public Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);
}
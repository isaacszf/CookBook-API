using Domain.Entities;
using Domain.Repositories.Recipe;

namespace Infra.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository
{
    private readonly CookBookDbContext _dbContext;

    public RecipeRepository(CookBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
}
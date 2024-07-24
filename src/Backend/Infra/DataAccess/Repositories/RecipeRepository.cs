using Domain.Dtos;
using Domain.Entities;
using Domain.Repositories.Recipe;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infra.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    private readonly CookBookDbContext _dbContext;

    public RecipeRepository(CookBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
    
    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        var userRecipesQuery = _dbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r =>
                r.Active &&
                r.UserId == user.Id);

        if (filters.Difficulties.Any()) 
            userRecipesQuery = userRecipesQuery.Where(r => r.Difficulty.HasValue && 
                                        filters.Difficulties.Contains(r.Difficulty.Value));
        
        if (filters.CookingTimes.Any()) 
            userRecipesQuery = userRecipesQuery.Where(r => r.CookingTime.HasValue && 
                                        filters.CookingTimes.Contains(r.CookingTime.Value));

        if (filters.DishTypes.Any())
            userRecipesQuery = userRecipesQuery.Where(r => r.DishTypes.Any(dishType => 
                filters.DishTypes.Contains(dishType.Type)));

        if (!filters.RecipeTitle_Ingredient.IsNullOrEmpty())
        {
            userRecipesQuery = userRecipesQuery.Where(r =>
                r.Title.Contains(filters.RecipeTitle_Ingredient!) ||
                r.Ingredients.Any(i => i.Item.Contains(filters.RecipeTitle_Ingredient!)));
        }
        
        return await userRecipesQuery.ToListAsync();
    }

    public async Task<Recipe?> GetById(User user, long recipeId)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.DishTypes)
            .Include(r => r.Instructions)
            .FirstOrDefaultAsync(r => 
                r.Active && 
                r.UserId.Equals(user.Id) &&
                r.Id.Equals(recipeId));
    }
}
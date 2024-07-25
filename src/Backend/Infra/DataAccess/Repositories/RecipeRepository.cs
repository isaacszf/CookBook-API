using Domain.Dtos;
using Domain.Entities;
using Domain.Repositories.Recipe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;

namespace Infra.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
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

    public async Task Delete(long recipeId)
    {
        var recipe = await _dbContext.Recipes.FindAsync(recipeId);

        _dbContext.Recipes.Remove(recipe!);
    }
    
    public void Update(Recipe recipe) => _dbContext.Recipes.Update(recipe);
    
    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipeQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => 
                r.Active && 
                r.UserId.Equals(user.Id) &&
                r.Id.Equals(recipeId));
    }

    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipeQuery()
            .FirstOrDefaultAsync(r => 
                r.Active && 
                r.UserId.Equals(user.Id) &&
                r.Id.Equals(recipeId));
    }

    public async Task<IList<Recipe>> GetFirstFive(User user)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => r.Active && r.UserId.Equals(user.Id))
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
            .ToListAsync();
    }
    
    private IIncludableQueryable<Recipe, ICollection<Instruction>> GetFullRecipeQuery()
    {
        return _dbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.DishTypes)
            .Include(r => r.Instructions);
    }
}
using Domain.Enums;

namespace Domain.Dtos;

public record FilterRecipesDto
{
    public string? RecipeTitle_Ingredient { get; init; }
    public IList<CookingTimeEnum> CookingTimes { get; init; } = [];
    public IList<CookingDifficultyEnum> Difficulties { get; init; } = [];
    public IList<DishTypeEnum> DishTypes { get; init; } = [];
}
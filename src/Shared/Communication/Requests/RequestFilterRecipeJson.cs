using Communication.Enums;

namespace Communication.Requests;

public class RequestFilterRecipeJson
{
    public string? RecipeTitle_Ingredient { get; set; }
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<CookingDifficulty> Difficulties { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
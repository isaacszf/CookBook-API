using Communication.Enums;

namespace Communication.Requests;

public class RequestRecipeJson
{
    public string Title { get; set; } = string.Empty;
    
    public CookingTime? CookingTime { get; set; }
    public CookingDifficulty? Difficulty { get; set; }

    public IList<string> Ingredients { get; set; } = [];
    public IList<RequestInstructionJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
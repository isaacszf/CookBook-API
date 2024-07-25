using Domain.Enums;

namespace Domain.Entities;

public class Recipe : Base
{
    public string Title { get; set; } = string.Empty;
    public CookingTimeEnum? CookingTime { get; set; }
    public CookingDifficultyEnum? Difficulty { get; set; }
    public ICollection<Instruction> Instructions { get; set; } = [];
    public ICollection<DishType> DishTypes { get; set; } = [];
    public ICollection<Ingredient> Ingredients { get; set; } = [];
    
    public long UserId { get; set; }
    public User? User { get; set; }
}
using Domain.Enums;

namespace Domain.Entities;

public class DishType : Base
{
    public DishTypeEnum Type { get; set; }
    
    public long RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}
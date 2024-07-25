namespace Domain.Entities;

public class Ingredient : Base
{
    public string Item { get; set; } = string.Empty;
    
    public long RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}
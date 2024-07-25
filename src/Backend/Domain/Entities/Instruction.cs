namespace Domain.Entities;

public class Instruction : Base
{
    public int Step { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public long RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}
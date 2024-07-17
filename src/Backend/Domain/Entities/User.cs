namespace Domain.Entities;

public class User : Base
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }
    
    public ICollection<Recipe>? Recipes { get; set; }
}
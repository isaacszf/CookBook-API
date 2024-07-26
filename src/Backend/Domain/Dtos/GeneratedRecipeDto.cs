using Domain.Enums;

namespace Domain.Dtos;

public class GeneratedRecipeDto
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<GeneratedInstructionDto> Instructions { get; set; } = [];
    public CookingTimeEnum CookingTime { get; set; }
}
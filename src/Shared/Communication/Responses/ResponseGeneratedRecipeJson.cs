using Communication.Enums;

namespace Communication.Responses;

public class ResponseGeneratedRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<ResponseGeneratedInstructionJson> Instructions { get; set; } = [];
    public CookingTime CookingTime { get; set; }
    public CookingDifficulty Difficulty { get; set; }
}
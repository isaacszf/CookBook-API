using System.Collections;

namespace Communication.Responses;

public class ResponseRecipesJson
{
    public IList<ResponseShortRecipeJson> Recipes { get; set; } = [];
}
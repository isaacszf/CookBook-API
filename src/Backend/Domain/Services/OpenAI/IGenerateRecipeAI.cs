using Domain.Dtos;

namespace Domain.Services.OpenAI;

public interface IGenerateRecipeAi
{
    public Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
}
using Communication.Enums;
using Communication.Requests;
using Communication.Responses;
using Domain.Services.OpenAI;
using Exceptions.Base;

namespace Application.UseCases.Recipe.Generate;

public class GenerateRecipeUseCase : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAi _generator;

    public GenerateRecipeUseCase(IGenerateRecipeAi generator)
    {
        _generator = generator;
    }
    
    public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson req)
    {
        Validate(req);

        var response = await _generator.Generate(req.Ingredients);

        return new ResponseGeneratedRecipeJson
        {
            Title = response.Title,
            Ingredients = response.Ingredients,
            Instructions = response.Instructions.Select(i => new ResponseGeneratedInstructionJson
            {
                Step = i.Step,
                Description = i.Description
            }).ToList(),
            CookingTime = (CookingTime) response.CookingTime,
            Difficulty = CookingDifficulty.Low
        };
    }

    private static void Validate(RequestGenerateRecipeJson req)
    {
        var validator = new GenerateRecipeValidator();
        var result = validator.Validate(req);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
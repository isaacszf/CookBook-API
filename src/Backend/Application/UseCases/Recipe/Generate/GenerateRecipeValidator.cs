using Communication.Requests;
using Domain.ValueObjects;
using Exceptions;
using FluentValidation;

namespace Application.UseCases.Recipe.Generate;

public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
{
    public GenerateRecipeValidator()
    {
        var maxNumIngredients = CookBookConstants.MaxIngredientsGenerateRecipe;

        RuleFor(r => r.Ingredients.Count).InclusiveBetween(1, maxNumIngredients)
            .WithMessage(ResourceMessageException.INVALID_NUM_INGREDIENTS);

        RuleFor(r => r.Ingredients).Must(i => i.Count == i.Select(c => c).Distinct().Count())
            .WithMessage(ResourceMessageException.DUPLICATE_INGREDIENTS);

        RuleFor(r => r.Ingredients).ForEach(rule =>
        {
            rule.Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Ingredient", ResourceMessageException.AT_LEAST_ONE_INGREDIENT);
                    return;
                }

                if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
                {
                    context.AddFailure("Ingredient", ResourceMessageException.INGREDIENT_NOT_FOLLOWING_PATTERN);
                }
            });
        });
    }
}
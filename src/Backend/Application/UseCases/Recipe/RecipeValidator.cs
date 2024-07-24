using Communication.Requests;
using Exceptions;
using FluentValidation;

namespace Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(r => r.Title).NotEmpty().WithMessage(ResourceMessageException.RECIPE_TITLE_EMPTY);
        RuleFor(r => r.CookingTime).IsInEnum().WithMessage(ResourceMessageException.COOKING_TIME_INVALID);
        RuleFor(r => r.Difficulty).IsInEnum().WithMessage(ResourceMessageException.COOKING_DIFFICULTY_INVALID);

        RuleFor(r => r.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMessageException.AT_LEAST_ONE_INGREDIENT);
        RuleFor(r => r.Instructions.Count).GreaterThan(0).WithMessage(ResourceMessageException.AT_LEAST_ONE_INSTRUCTION);
        RuleForEach(r => r.Instructions).NotEmpty().WithMessage(ResourceMessageException.AT_LEAST_ONE_INSTRUCTION);
        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessageException.INVALID_DISH_TYPE);
        RuleForEach(r => r.Instructions).ChildRules(instruction =>
        {
            instruction.RuleFor(i => i.Step).GreaterThan(0).WithMessage(ResourceMessageException.INVALID_STEP);
            instruction.RuleFor(i => i.Description)
                .NotEmpty()
                .WithMessage(ResourceMessageException.INSTRUCTION_EMPTY)
                .MaximumLength(2000)
                .WithMessage(ResourceMessageException.INSTRUCTION_EXCEEDS_LIMIT);
        });

        RuleFor(r => r.Instructions)
            .Must(instruction => instruction.Select(i => i.Step).Distinct().Count() == instruction.Count)
            .WithMessage(ResourceMessageException.SAME_INSTRUCTION_ORDER);
    }
}
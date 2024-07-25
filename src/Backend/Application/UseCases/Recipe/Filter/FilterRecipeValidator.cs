using Communication.Requests;
using Exceptions;
using FluentValidation;

namespace Application.UseCases.Recipe.Filter;

public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
{
    public FilterRecipeValidator()
    {
        RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage(ResourceMessageException.COOKING_TIME_INVALID);
        RuleForEach(r => r.Difficulties).IsInEnum().WithMessage(ResourceMessageException.COOKING_DIFFICULTY_INVALID);
        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessageException.INVALID_DISH_TYPE);
    }
}
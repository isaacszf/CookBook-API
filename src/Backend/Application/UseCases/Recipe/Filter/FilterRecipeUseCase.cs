using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Dtos;
using Domain.Enums;
using Domain.Repositories.Recipe;
using Domain.Services.LoggedUser;
using Exceptions.Base;

namespace Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public FilterRecipeUseCase(
        IRecipeReadOnlyRepository recipeRepository,
        ILoggedUser loggedUser, 
        IMapper mapper
        )
    {
        _recipeRepository = recipeRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    
    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson req)
    {
        Validate(req);

        var loggedUser = await _loggedUser.User();

        var filteredRecipes = await _recipeRepository.Filter(loggedUser, new FilterRecipesDto
        {
            RecipeTitle_Ingredient = req.RecipeTitle_Ingredient,
            CookingTimes = req.CookingTimes.Distinct().Select(c => ((CookingTimeEnum)c)).ToList(),
            Difficulties = req.Difficulties.Distinct().Select(d => ((CookingDifficultyEnum)d)).ToList(),
            DishTypes = req.DishTypes.Distinct().Select(d => ((DishTypeEnum)d)).ToList(),
        });

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(filteredRecipes)
        };
    }

    private static void Validate(RequestFilterRecipeJson req)
    {
        var validator = new FilterRecipeValidator();
        var result = validator.Validate(req);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(err => err.ErrorMessage).Distinct().ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}
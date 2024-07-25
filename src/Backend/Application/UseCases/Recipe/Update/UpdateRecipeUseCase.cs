using AutoMapper;
using Communication.Requests;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Recipe;
using Domain.Services.LoggedUser;
using Exceptions;
using Exceptions.Base;

namespace Application.UseCases.Recipe.Update;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRecipeUseCase(
            ILoggedUser loggedUser,
            IRecipeUpdateOnlyRepository recipeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
    {
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task Execute(long id, RequestRecipeJson req)
    {
        Validate(req);

        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById(loggedUser, id);

        if (recipe is null) throw new NotFoundException(ResourceMessageException.RECIPE_NOT_FOUND);
        
        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes.Clear();

        _mapper.Map(req, recipe);

        var instructions = req.Instructions
            .OrderBy(i=> i.Step)
            .ToList();
        for (var i = 0; i < instructions.Count; i++) instructions.ElementAt(i).Step = i + 1;
        recipe.Instructions = _mapper.Map<IList<Instruction>>(instructions);
        
        _recipeRepository.Update(recipe);
        await _unitOfWork.Commit();
    }

    private static void Validate(RequestRecipeJson req)
    {
        var result = new RecipeValidator().Validate(req);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList());
    }
}
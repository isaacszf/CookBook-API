using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Repositories.Recipe;
using Domain.Services.LoggedUser;
using Exceptions.Base;

namespace Application.UseCases.Recipe.Register;

public class CreateRecipeUseCase : ICreateRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public CreateRecipeUseCase(
        IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser,
        IMapper mapper
            )
    {
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    
    public async Task<ResponseCreatedRecipeJson> Execute(RequestRecipeJson req)
    {
        Validate(req);

        var loggedUser = await _loggedUser.User();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(req);
        recipe.UserId = loggedUser.Id;
        
        // handling instructions
        var instructions = req.Instructions
            .OrderBy(i=> i.Step)
            .ToList();
        for (var i = 0; i < instructions.Count; i++) instructions.ElementAt(i).Step = i + 1;
        recipe.Instructions = _mapper.Map<IList<Instruction>>(instructions);

        // Saving
        await _recipeWriteOnlyRepository.Add(recipe);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseCreatedRecipeJson>(recipe);
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
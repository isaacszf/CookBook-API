using Domain.Repositories;
using Domain.Repositories.Recipe;
using Domain.Services.LoggedUser;
using Exceptions;
using Exceptions.Base;

namespace Application.UseCases.Recipe.Delete;

public class DeleteRecipeByIdUseCase : IDeleteRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRecipeByIdUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository recipeReadOnlyRepository,
            IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
            IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();
        
        var recipe = await _recipeReadOnlyRepository.GetById(loggedUser, recipeId);
        if (recipe is null) throw new NotFoundException(ResourceMessageException.RECIPE_NOT_FOUND);

        await _recipeWriteOnlyRepository.Delete(recipeId);
        await _unitOfWork.Commit();
    }
}
using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Recipe;
using Domain.Services.LoggedUser;
using Exceptions;
using Exceptions.Base;

namespace Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCaseUseCase : IGetRecipeByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetRecipeByIdUseCaseUseCase(
            IRecipeReadOnlyRepository recipeRepository,
            ILoggedUser loggedUser,
            IMapper mapper
        )
    {
        _recipeRepository = recipeRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    
    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById(loggedUser, recipeId);

        if (recipe is null) throw new NotFoundException(ResourceMessageException.RECIPE_NOT_FOUND);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}
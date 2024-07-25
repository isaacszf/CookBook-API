using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Recipe;
using Domain.Services.LoggedUser;

namespace Application.UseCases.Dashboard.Get;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _recipeRepository;
    private readonly IMapper _mapper;

    public GetDashboardUseCase(
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository recipeRepository,
            IMapper mapper
        )
    {
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _mapper = mapper;
    }
    
    public async Task<ResponseRecipesJson> Execute()
    {
        var loggedUser = await _loggedUser.User();
        var recipes = await _recipeRepository.GetFirstFive(loggedUser);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(recipes)
        };
    }
}
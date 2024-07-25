using Communication.Responses;

namespace Application.UseCases.Dashboard.Get;

public interface IGetDashboardUseCase
{
    public Task<ResponseRecipesJson> Execute();
}
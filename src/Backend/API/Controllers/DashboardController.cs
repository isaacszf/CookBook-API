using API.Attributes;
using Application.UseCases.Dashboard.Get;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AuthenticatedUser]
public class DashboardController : CookBookBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get([FromServices] IGetDashboardUseCase useCase)
    {
        var res = await useCase.Execute();
        if (res.Recipes.Any()) return Ok(res);

        return NoContent();
    }
}
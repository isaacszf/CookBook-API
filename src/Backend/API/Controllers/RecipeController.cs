using API.Attributes;
using Application.UseCases.Recipe.Filter;
using Application.UseCases.Recipe.Register;
using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AuthenticatedUser]
public class RecipeController : CookBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCreatedRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
            [FromServices] ICreateRecipeUseCase useCase,
            [FromBody] RequestRecipeJson req
        )
    {
        var res = await useCase.Execute(req);
        return Created(string.Empty, res);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter(
            [FromServices] IFilterRecipeUseCase useCase,
            [FromBody] RequestFilterRecipeJson req
        )
    {
        var res = await useCase.Execute(req);

        if (res.Recipes.Any()) return Ok(res);
        return NoContent();
    }
}
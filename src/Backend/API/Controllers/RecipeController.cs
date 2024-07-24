using API.Attributes;
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
}
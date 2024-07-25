using API.Attributes;
using API.Binders;
using Application.UseCases.Recipe.Delete;
using Application.UseCases.Recipe.Filter;
using Application.UseCases.Recipe.GetById;
using Application.UseCases.Recipe.Register;
using Application.UseCases.Recipe.Update;
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

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
            [FromServices] IUpdateRecipeUseCase useCase,
            [FromBody] RequestRecipeJson req,
            [FromRoute] [ModelBinder(typeof(CookBookIdBinder))] long id
        )
    {
        await useCase.Execute(id, req);
        return Ok();
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

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
            [FromServices] IGetRecipeByIdUseCase useCase,
            [FromRoute] [ModelBinder(typeof(CookBookIdBinder))] long id
        )
    {
        var res = await useCase.Execute(id);
        return Ok(res);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
            [FromServices] IDeleteRecipeByIdUseCase useCase,
            [FromRoute] [ModelBinder(typeof(CookBookIdBinder))] long id
        )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
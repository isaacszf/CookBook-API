using API.Attributes;
using Application.UseCases.User.Profile;
using Application.UseCases.User.Register;
using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController: CookBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var res = await useCase.Execute(request);
        return Created(string.Empty, res);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var res = await useCase.Execute();
        return Ok(res);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> UpdateUserProfile(
        [FromServices] IUpdateProfileUseCase useCase,
        [FromBody] RequestUpdateUserJson req)
    {
        await useCase.Execute(req);
        return NoContent();
    }
}
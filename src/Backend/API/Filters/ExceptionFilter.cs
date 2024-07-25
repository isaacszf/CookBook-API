using System.Net;
using Communication.Responses;
using Exceptions;
using Exceptions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class ExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CookBookException) HandleProjectException(context);
        else HandleUnknownException(context);
    }

    // TO-DO - make this better
    private static void HandleProjectException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ErrorOnValidationException exp:
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exp.ErrorsMessages));
                break;
            case InvalidLoginException:
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
                break;
            case NotFoundException:
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));
                break;
        }
    }
    
    private static void HandleUnknownException(ExceptionContext context)
    {
        var exp = context.Exception;
        Console.WriteLine(exp.ToString());
        
        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessageException.UNKNOWN_ERR));
    }
}
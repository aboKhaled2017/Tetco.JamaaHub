using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Services;

public class CustomExceptionMiddleWare : IMiddleware
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionMiddleWare()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (_exceptionHandlers.ContainsKey(ex.GetType()))
            {
                await _exceptionHandlers[ex.GetType()].Invoke(context, ex);
            }
            else
            {
               await HandleInternalServerErrorException(context, ex);
            }
        }
    }
    private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }


    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        });
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        });
    }

    private async Task HandleInternalServerErrorException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail=ex.Message
        });
    }
}

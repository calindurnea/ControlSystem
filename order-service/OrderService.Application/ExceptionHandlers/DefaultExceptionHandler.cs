using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;

namespace OrderService.Application.ExceptionHandlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DefaultExceptionHandler> _logger;

    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
#pragma warning disable CA1848
        _logger.LogError(exception, "An error occurred");
#pragma warning restore CA1848

        switch (exception)
        {
            case NotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(BuildProblemDetails(httpContext, exception, HttpStatusCode.NotFound), cancellationToken).ConfigureAwait(false);
                break;
            default:
                await httpContext.Response.WriteAsJsonAsync(BuildProblemDetails(httpContext, exception, HttpStatusCode.InternalServerError), cancellationToken).ConfigureAwait(false);
                break;
        }

        return true;
    }

    private static ProblemDetails BuildProblemDetails(HttpContext httpContext, Exception exception, HttpStatusCode statusCodes)
    {
        return new ProblemDetails
        {
            Status = (int)statusCodes,
            Type = exception.GetType().Name,
            Title = "An error occurred",
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };
    }
}

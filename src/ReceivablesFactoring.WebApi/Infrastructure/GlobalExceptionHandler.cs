using Microsoft.AspNetCore.Diagnostics;
using ReceivablesFactoring.Domain.Exceptions;
using System.Net;

namespace ReceivablesFactoring.WebApi.Infrastructure;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ExceptionResponse response = exception switch
        {
            ValidationFailureException e => new ExceptionResponse(HttpStatusCode.BadRequest, "A validation error ocurred.", e.Errors),
            NotFoundException e => new ExceptionResponse(HttpStatusCode.NotFound, "Resource not found.", [e.Message]),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.", [])
        };

        logger.LogError(response.Message);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}

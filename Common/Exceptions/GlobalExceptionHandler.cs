using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Exceptions
{
    public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandle exception occured");
            httpContext.Response.StatusCode = exception switch
            {
                AppException appException => (int)appException.Detail.HttpStatusCode,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = exception switch
            {
                AppException appEx => new ProblemDetails
                {
                    Status = httpContext.Response.StatusCode,
                    Type = appEx.GetType().Name,
                    Title = "Application error",
                    Detail = appEx.Detail.Message
                },
                _ => new ProblemDetails
                {
                    Status = httpContext.Response.StatusCode,
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message
                }
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
        }
    }
}

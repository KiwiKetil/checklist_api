using Microsoft.AspNetCore.Mvc;

namespace Checklist_API.Features.ExceptionHandling;

public class ExceptionHandler(ILogger<ExceptionHandler> logger)
{
    private readonly ILogger<ExceptionHandler> _logger = logger;  

    public async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An error occurred - {@Machine} {@TraceId}",
            Environment.MachineName,
            System.Diagnostics.Activity.Current?.Id);

        var (statusCode, title) = ex switch
        {
            CustomExceptions.UserAlreadyExistsException => (StatusCodes.Status409Conflict, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = ex.Message,
            Status = statusCode,
            Extensions = { ["traceId"] = System.Diagnostics.Activity.Current?.Id }
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}

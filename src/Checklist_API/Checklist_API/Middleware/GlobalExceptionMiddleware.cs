using static Checklist_API.Extensions.CustomExceptions;

namespace Checklist_API.Middleware;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;
  
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong - test exception {@Machine} {@TraceId}",
            Environment.MachineName,
            System.Diagnostics.Activity.Current?.Id);

            var statusCode = ex switch
            {
                UserAlreadyExistsException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var title = ex switch
            {
                UserAlreadyExistsException => "User already exists",
                _ => "Something terrible has happened!!"
            };

            await Results.Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                {
                    { "traceId", System.Diagnostics.Activity.Current?.Id },
                })
                .ExecuteAsync(context);
        }

    }
}

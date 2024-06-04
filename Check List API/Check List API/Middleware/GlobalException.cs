
namespace Check_List_API.Middleware;

public class GlobalException : IMiddleware
{
    private readonly ILogger<GlobalException> _logger;

    public GlobalException(ILogger<GlobalException> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Noe gikk galt- test exception {@Machine} {@TraceId}",
            Environment.MachineName,
            System.Diagnostics.Activity.Current?.Id);

            await Results.Problem(
                title: "Noe fryktelig har skjedd!!",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    { "traceId", System.Diagnostics.Activity.Current?.Id },
                })
                .ExecuteAsync(context);
        }
    }
}

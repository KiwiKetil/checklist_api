using Checklist_API.Features.ExceptionHandling;
using static Checklist_API.Features.ExceptionHandling.CustomExceptions;

namespace Checklist_API.Middleware;
public class GlobalExceptionMiddleware(ExceptionHandler exceptionHandler) : IMiddleware
{
    private readonly ExceptionHandler _exceptionHandler = exceptionHandler;    

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await _exceptionHandler.HandleException (context, ex);
        }
    }
}
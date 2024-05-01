using GEH.Exceptions;
using Microsoft.AspNetCore.Http;

namespace GEH;

public class GlobalErrorHandler
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(ex.Message);
            return;
        }
        catch (FailedException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(ex.Message);
            return;
        }
    }
}
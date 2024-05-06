using GEH.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace GEH;

public static class GlobalErrorHandlerExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder builder, string projectName)
    {
        return builder.UseMiddleware<GlobalErrorHandlerMiddleware>(projectName);
    }
}

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAlertaGram _alertaGram;
    private readonly string _projectName;

    public GlobalErrorHandlerMiddleware(RequestDelegate next, IAlertaGram alertaGram, string projectName)
    {
        _next = next;
        _alertaGram = alertaGram;
        _projectName = projectName;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(ex, 404, context);
            return;
        }
        catch (FailedException ex)
        {
            await HandleExceptionAsync(ex, 400, context);
            return;
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, 500, context);
            return;
        }
    }

    public async Task HandleExceptionAsync(Exception ex, int statusCode, HttpContext context)
    {
        if (context == null)
            throw new InvalidOperationException("HttpContextAccessor.HttpContext is null.");

        context.Response.ContentType = "text/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(ex.Message);

        string source = ex.StackTrace?.Split("\n")[0] ?? "";

        var request = context.Request;
        var builder = new StringBuilder();
        builder.AppendLine($"Method: [{request.Method}]");
        string path = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        builder.AppendLine($"Path: {path}");
        string ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "";

        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            ipAddress = context.Request.Headers["X-Forwarded-For"]!;
            ipAddress = ipAddress.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim() ?? "";
        }

        if (context.Request.Headers.ContainsKey("X-Real-IP") && string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Request.Headers["X-Real-IP"]!;
        }

        if (context.Request.Headers.ContainsKey("REMOTE_ADDR") && string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Request.Headers["REMOTE_ADDR"]!;
        }
        builder.AppendLine($"IP Address: {ipAddress}");
        builder.AppendLine($"User Agent: {request.Headers["User-Agent"]}");

        string message = $"""
        🛑{ex.GetType().Name}: {ex.Message}
            
        🪲Source: {source}

        📡Request
        {builder.ToString()}
        """;

        try
        {
            await _alertaGram.NotifyAsync(message, _projectName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
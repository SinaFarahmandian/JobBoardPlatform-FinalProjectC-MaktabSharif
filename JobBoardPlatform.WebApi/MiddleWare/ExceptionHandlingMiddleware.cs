using JobBoardPlatform.Buisiness.Common.Exceptions;

namespace JobBoardPlatform.WebApi.MiddleWare;

using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (NotFoundException ex) { await Write(context, 404, ex.Message); }
        catch (ForbiddenAccessException ex) { await Write(context, 403, ex.Message); }
        catch (InvalidStatusTransitionException ex) { await Write(context, 400, ex.Message); }
        catch (ArgumentException ex) { await Write(context, 400, ex.Message); }
        catch (Exception) { await Write(context, 500, "خطای غیرمنتظره‌ای رخ داد"); }
    }

    private static async Task Write(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
    }
}
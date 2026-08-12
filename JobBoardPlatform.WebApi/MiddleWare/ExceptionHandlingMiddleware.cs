using System.Text.Json;
using JobBoardPlatform.Buisiness.Common;
using JobBoardPlatform.Buisiness.Common.Exceptions;

namespace JobBoardPlatform.WebApi.MiddleWare;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var (status, message) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                ForbiddenAccessException => (StatusCodes.Status403Forbidden, ex.Message),
                BadRequestException => (StatusCodes.Status400BadRequest, ex.Message),
                InvalidStatusTransitionException => (StatusCodes.Status400BadRequest, ex.Message),
                ArgumentException => (StatusCodes.Status400BadRequest, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            if (status == StatusCodes.Status500InternalServerError)
                _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.FailureResponse(message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

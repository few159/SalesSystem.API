using System.Net;
using System.Text.Json;

namespace SalesSystem.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = ex switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new
            {
                type = ex.GetType().Name,
                error = GetErrorMessage(response.StatusCode),
                detail = ex.Message
            });

            await response.WriteAsync(result);
        }
    }

    private static string GetErrorMessage(int statusCode) => statusCode switch
    {
        400 => "Invalid request",
        401 => "Unauthorized",
        404 => "Resource not found",
        500 => "An unexpected error occurred",
        _ => "Unhandled error"
    };
}
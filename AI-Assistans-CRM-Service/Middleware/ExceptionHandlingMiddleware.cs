using System.Net;
using System.Text.Json;

namespace AI_Assistans_CRM_Service.Middleware;

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
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await WriteErrorResponse(context, "Unauthorized", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await WriteErrorResponse(context, "Not Found", ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await WriteErrorResponse(context, "Bad Request", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await WriteErrorResponse(context, "Internal Server Error", "An unexpected error occurred");
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, string title, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        var problem = new
        {
            type = $"https://httpstatuses.com/{context.Response.StatusCode}",
            title,
            status = context.Response.StatusCode,
            detail,
            instance = context.Request.Path
        };
        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}

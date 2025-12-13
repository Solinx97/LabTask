using LabTask.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LabTask.API.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var eventId = Guid.NewGuid().ToString();

        var (status, title, detail, logLevel) = ex switch
        {
            DomainException => (
                StatusCodes.Status422UnprocessableEntity,
                "Business rule violation",
                ex.Message,
                LogLevel.Warning
            ),
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Invalid request",
                ex.Message,
                LogLevel.Warning
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "An unexpected error occurred.",
                LogLevel.Error
            )
        };

        _logger.Log(logLevel, new EventId(eventId.GetHashCode()), ex, "{Title}. EventId: {EventId}", title, eventId);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["eventId"] = eventId;

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        await context.Response.WriteAsJsonAsync(problem, options, "application/problem+json");
    }
}

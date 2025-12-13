using Microsoft.AspNetCore.Mvc;

namespace WebApp.Server.Middlewares;

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
        catch (HttpRequestException ex)
        {
            var eventId = Guid.NewGuid().ToString();

            _logger.LogError(ex, "Transport error to downstream API. EventId: {EventId}", eventId);

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status503ServiceUnavailable,
                Title = "Downstream service unavailable",
                Detail = "Unable to reach downstream API.",
                Instance = context.Request.Path
            };
            problem.Extensions["eventId"] = eventId;

            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            var eventId = Guid.NewGuid().ToString();

            _logger.LogError(ex, "Unexpected proxy error. EventId: {EventId}", eventId);

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Proxy internal error",
                Detail = "An unexpected error occurred in the proxy.",
                Instance = context.Request.Path
            };
            problem.Extensions["eventId"] = eventId;

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}

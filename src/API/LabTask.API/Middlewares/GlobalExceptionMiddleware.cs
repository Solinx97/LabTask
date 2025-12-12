using LabTask.API.Enums;

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
        catch (ArgumentOutOfRangeException ex)
        {
            var eventId = DateTime.UtcNow.Ticks.ToString();

            _logger.LogError(new EventId(int.Parse(eventId[^9..])), ex,
                ex.Message, eventId);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                id = eventId,
                type = ExceptionType.ArgumentOutOfRangeException,
                data = new
                {
                    message = $"Internal server error ID = {eventId}"
                }
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (ArgumentNullException ex)
        {
            var eventId = DateTime.UtcNow.Ticks.ToString();

            _logger.LogError(new EventId(int.Parse(eventId[^9..])), ex,
                ex.Message, eventId);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                id = eventId,
                type = ExceptionType.ArgumentNullException,
                data = new
                {
                    message = $"Internal server error ID = {eventId}"
                }
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (ArgumentException ex)
        {
            var eventId = DateTime.UtcNow.Ticks.ToString();

            _logger.LogError(new EventId(int.Parse(eventId[^9..])), ex,
                ex.Message, eventId);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                id = eventId,
                type = ExceptionType.ArgumentException,
                data = new
                {
                    message = $"Internal server error ID = {eventId}"
                }
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            var eventId = DateTime.UtcNow.Ticks.ToString();

            _logger.LogError(new EventId(int.Parse(eventId[^9..])), ex,
                ex.Message, eventId);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                id = eventId,
                type = ExceptionType.Exception,
                data = new
                {
                    message = $"Internal server error ID = {eventId}"
                }
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}

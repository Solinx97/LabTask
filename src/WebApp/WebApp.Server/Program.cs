using Serilog;
using Serilog.Events;
using WebApp.Server.Attributes;
using WebApp.Server.Consts;
using WebApp.Server.Helpers;
using WebApp.Server.Interfaces;
using WebApp.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddScoped<RequireAccessTokenAttribute>();

builder.Services.Configure<APIServer>(builder.Configuration.GetSection("APIServer"));
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/proxy.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();

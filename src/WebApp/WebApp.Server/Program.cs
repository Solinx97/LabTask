using WebApp.Server.Attributes;
using WebApp.Server.Consts;
using WebApp.Server.Helpers;
using WebApp.Server.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddScoped<RequireAccessTokenAttribute>();

builder.Services.Configure<APIServer>(builder.Configuration.GetSection("APIServer"));
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

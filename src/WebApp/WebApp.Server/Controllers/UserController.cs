using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Server.Attributes;
using WebApp.Server.Consts;
using WebApp.Server.Enums;
using WebApp.Server.Interfaces;
using WebApp.Server.Models;

namespace WebApp.Server.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<UserController> _logger;
    private readonly Authentication _authentication;

    public UserController(IOptions<APIServer> server, IHttpClientHelper httpClient, ILogger<UserController> logger,
        IOptions<Authentication> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _authentication = options.Value;

        _httpClient.APIUrl = server.Value.User;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationModel item)
    {
        var response = await _httpClient.PostAsync("User/register", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("User registry successfully.");

            return Ok();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem during Registration: {Title} - {Detail}", problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API during Registration. Status: {StatusCode}", response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel item)
    {
        var response = await _httpClient.PostAsync("User/login", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("User login successfully.");

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            HttpContext.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), loginResponse.Token, new CookieOptions
            {
                Domain = _authentication.CookieDomain,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(_authentication.ExpiresInHours),
            });

            return Ok(loginResponse.User);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem during Login: {Title} - {Detail}", problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API during Login. Status: {StatusCode}", response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpPost("logout")]
    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    public async Task<IActionResult> Logout()
    {
        var response = await _httpClient.PostAsync("User/logout", null);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("User logout successfully.");

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.AccessToken));

            return Ok();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem during Logout: {Title} - {Detail}", problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API during Logout. Status: {StatusCode}", response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet]
    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    public async Task<IActionResult> GetAll()
    {
        var response = await _httpClient.GetAsync("User");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Users extracted successfully.");

            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            return Ok(users);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Users: {Title} - {Detail}", problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Users. Status: {StatusCode}", response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet("refresh")]
    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    public async Task<IActionResult> Refresh()
    {
        var response = await _httpClient.GetAsync("User/refresh");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("User refresh successfully.");

            var user = await response.Content.ReadFromJsonAsync<UserModel>();

            return Ok(user);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem during Refresh: {Title} - {Detail}", problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API during Refresh. Status: {StatusCode}", response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }
}

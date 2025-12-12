using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
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
        try
        {
            var responseMessage = await _httpClient.PostAsync("User/register", JsonContent.Create(item));
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during Registry a new user. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during Registry a new user. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel item)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("User/login", JsonContent.Create(item));
            responseMessage.EnsureSuccessStatusCode();

            var loginResponse = await responseMessage.Content.ReadFromJsonAsync<LoginResponseModel>();

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
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during Login. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during Login. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost("logout")]
    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("User/logout", null);
            responseMessage.EnsureSuccessStatusCode();

            HttpContext.Response.Cookies.Delete(nameof(AuthenticationCookie.AccessToken));

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during Logout. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during Logout. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("refresh")]
    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("User/refresh");
            responseMessage.EnsureSuccessStatusCode();

            var user = await responseMessage.Content.ReadFromJsonAsync<UserModel>();

            return Ok(user);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during Logout. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during Logout. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}

using LabTask.UserAPI.Consts;
using LabTask.UserAPI.DTOs;
using LabTask.UserAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabTask.UserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<Authentication> options) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly Authentication authentication = options.Value;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return Unauthorized();

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);

        return Ok(new { User = user, Token = token });
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok();
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var header = HttpContext.Request.Headers.Authorization;
        var token = header.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        var userId = GetUserId(token);
        var user = await _userManager.FindByIdAsync(userId);

        return Ok(user);
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("scope", authentication.Scopes),
        };

        var audencies = authentication.Audiences.Split(',');
        foreach (var auden in audencies)
        {
            claims = [.. claims, new Claim(JwtRegisteredClaimNames.Aud, auden)];
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authentication.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: authentication.Issuer,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GetUserId(string tokenAsString)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenAsString);

        var userId = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        return userId;
    }
}
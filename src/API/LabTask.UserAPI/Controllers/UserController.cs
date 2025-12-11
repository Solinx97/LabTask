using LabTask.UserAPI.Consts;
using LabTask.UserAPI.DTOs;
using LabTask.UserAPI.Entities;
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
public class UserController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IOptions<Authentication> options) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly Authentication authentication = options.Value;
    private readonly IConfiguration _configuration = configuration;

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

        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Ok(new { UserId = userId });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authentication.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: authentication.Issuer,
            audience: authentication.Audiences,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

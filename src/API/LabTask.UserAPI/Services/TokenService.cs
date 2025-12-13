using LabTask.UserAPI.Consts;
using LabTask.UserAPI.Interfaces;
using LabTask.UserDAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabTask.UserAPI.Services;

internal class TokenService(IOptions<Authentication> options) : ITokenService
{
    private readonly Authentication authentication = options.Value;

    public string GenerateJwtToken(ApplicationUser user)
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

    public string GetUserId(string tokenAsString)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenAsString);

        var userId = token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        return userId;
    }
}

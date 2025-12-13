using LabTask.UserAPI.Entities;

namespace LabTask.UserAPI.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(ApplicationUser user);

    string GetUserId(string tokenAsString);
}

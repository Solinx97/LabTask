using LabTask.UserAPI.Entities;

namespace LabTask.UserAPI.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default);
}

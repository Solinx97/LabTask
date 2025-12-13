using LabTask.UserDAL.Entities;

namespace LabTask.UserDAL.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ApplicationUser> GetByIdAsync(string id, CancellationToken ct = default);
}

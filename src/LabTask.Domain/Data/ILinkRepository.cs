using LabTask.Domain.Aggregates;

namespace LabTask.Domain.Data;

public interface ILinkRepository : IGenericRepository<Link>
{
    Task<IEnumerable<Link>> GetByOwnerIdAsync(Guid ownerId, int page, int pageSize, CancellationToken ct = default);

    Task<IEnumerable<Link>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
}

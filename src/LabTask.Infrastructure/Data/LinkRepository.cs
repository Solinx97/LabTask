using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class LinkRepository(AppDbContext dbContext) : GenericRepository<Link>(dbContext), ILinkRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Link>> GetByOwnerIdAsync(Guid ownerId, int page, int pageSize, CancellationToken ct = default)
    {
        var links = await _dbContext.Link
            .AsNoTracking()
            .Where(d => d.OwnerId == ownerId)
            .OrderBy(d => d.Id)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return links;
    }

    public async Task<IEnumerable<Link>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var links = await _dbContext.Link
            .AsNoTracking()
            .Where(d => d.ToUserId == userId)
            .OrderBy(d => d.Id)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return links;
    }
}

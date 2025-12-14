using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class LinkRepository(AppDbContext dbContext) : GenericRepository<Link>(dbContext), ILinkRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Link>> GetByOwnerIdAsync(Guid ownerId, Guid documentId, int page, int pageSize, CancellationToken ct = default)
    {
        var links = await _dbContext.Link
            .AsNoTracking()
            .Where(l => l.OwnerId == ownerId && l.DocumentId == documentId)
            .OrderBy(l => l.Id)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return links;
    }

    public async Task<IEnumerable<Link>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var links = await _dbContext.Link
            .AsNoTracking()
            .Where(l => l.ToUserId == userId)
            .OrderBy(l => l.Id)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return links;
    }
}

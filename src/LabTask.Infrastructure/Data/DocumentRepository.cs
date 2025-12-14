using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Domain.Helpers;
using LabTask.Infrastructure.Exceptions;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class DocumentRepository(AppDbContext dbContext) : GenericRepository<Document>(dbContext), IDocumentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Document> GetByIdAsync(Guid id, Guid commentId, CancellationToken ct = default)
    {
        var entity = await _dbContext.Document
            .Where(d => d.Id == id)
            .Include(d => d.Comments.Where(c => c.Id == commentId))
            .FirstOrDefaultAsync(ct) 
                ?? throw new EntityNotFoundException(typeof(Document), id);

        return entity;
    }

    public async Task<IEnumerable<Statistic>> GetStatisticsAsync(Guid userId, DateTimeOffset startedAt, DateTimeOffset finishedAt, CancellationToken ct = default)
    {
        var documents = await _dbContext.Document
            .AsNoTracking()
            .Where(doc => doc.UserId == userId && 
                (doc.CreatedAt >= startedAt && doc.CreatedAt <= finishedAt))
            .Select(doc => new
            {
                doc.CreatedAt,
                doc.UpdatedAt,
                doc.ExpireAt
            })
            .ToListAsync(ct);

        var statistic = documents
            .GroupBy(doc => doc.CreatedAt.Year)
            .Select(g => new Statistic
            {
                Year = g.Key,
                CreatedAtCount = g.Count(),
                UpdatedAtCount = g.Count(x => x.UpdatedAt != null && (x.UpdatedAt.Value >= startedAt && x.UpdatedAt.Value <= finishedAt)),
                ExpiredAtCount = g.Count(x => x.ExpireAt >= startedAt && x.ExpireAt <= finishedAt),
            })
            .OrderBy(s => s.Year)
            .ToList();

        return statistic;
    }

    public async Task<IEnumerable<Statistic>> GetStatisticsAsync(Guid userId, CancellationToken ct = default)
    {
        var documents = await _dbContext.Document
            .AsNoTracking()
            .Where(doc => doc.UserId == userId)
            .Select(doc => new
            {
                doc.CreatedAt,
                doc.UpdatedAt,
                doc.ExpireAt
            })
            .ToListAsync(ct);

        var statistic = documents
            .GroupBy(doc => doc.CreatedAt.Year)
            .Select(g => new Statistic
            {
                Year = g.Key,
                CreatedAtCount = g.Count(),
                UpdatedAtCount = g.Count(x => x.UpdatedAt != null && x.UpdatedAt.Value.Year == g.Key),
                ExpiredAtCount = g.Count(x => x.ExpireAt.Year == g.Key),
            })
            .OrderBy(s => s.Year)
            .ToList();

        return statistic;
    }

    public async Task<IEnumerable<Document>> GetActualDocumentsAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        var documents = await _dbContext.Document
            .Where(d => d.UserId == userId && d.ExpireAt >= now)
            .OrderBy(d => d.ExpireAt)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return documents;
    }


    public async Task<IEnumerable<Document>> GetExpiredDocumentsAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        var documents = await _dbContext.Document
            .AsNoTracking()
            .Where(d => d.UserId == userId && d.ExpireAt < now)
            .OrderBy(d => d.ExpireAt)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return documents;
    }

    public async Task<IEnumerable<Document>> GetDocumentByNameAsync(Guid userId, string name, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        var documents = await _dbContext.Document
            .Where(o => o.UserId == userId && o.Name.StartsWith(name) && o.ExpireAt >= now)
            .OrderBy(d => d.Id)
            .ToListAsync(ct);

        return documents;
    }

    public async Task<IEnumerable<int>> GetCreatedYearsAsync(Guid userId, CancellationToken ct = default)
    {
        var years = await _dbContext.Document
            .Where(o => o.UserId == userId)
            .OrderBy(d => d.Id)
            .Select(d => d.CreatedAt.Year)
            .Distinct()
            .ToListAsync(ct);

        return years;
    }
}

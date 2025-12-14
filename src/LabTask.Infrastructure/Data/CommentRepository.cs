using LabTask.Domain.Data;
using LabTask.Domain.Entities;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class CommentRepository(AppDbContext dbContext) : GenericRepository<Comment>(dbContext), ICommentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Comment>> GetByDocumentIdAsync(Guid documentId, int page, int pageSize, CancellationToken ct = default)
    {
        var comments = await _dbContext.Comment
            .Where(c => c.DocumentId == documentId)
            .OrderBy(c => c.Id)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return comments;
    }
}

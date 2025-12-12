using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Infrastructure.Exceptions;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class DocumentRepository(AppDbContext dbContext) : GenericRepository<Document>(dbContext), IDocumentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Document> GetByIdAsync(Guid id, Guid commentId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Document
            .Where(d => d.Id == id)
            .Include(d => d.Comments.Where(c => c.Id == commentId))
            .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new EntityNotFoundException(typeof(Document), id);

        return entity;
    }
}

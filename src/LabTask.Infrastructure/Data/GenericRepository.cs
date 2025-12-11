using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Infrastructure.Exceptions;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class GenericRepository(AppDbContext dbContext) : IGenericRepository<Document>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(Document document)
    {
        await _dbContext.Document.AddAsync(document);
    }

    public async Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Document
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Document), id);

        return entity;
    }

    public async Task DeletedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Document
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Document), id);

        _dbContext.Document.Remove(entity);
    }
}

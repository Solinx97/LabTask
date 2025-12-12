using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Domain.Interfaces;
using LabTask.Infrastructure.Exceptions;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Data;

internal class GenericRepository<TModel>(AppDbContext dbContext) : IGenericRepository<TModel>
    where TModel : class, IEntityId
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(TModel item)
    {
        await _dbContext.Set<TModel>().AddAsync(item);
    }

    public async Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<TModel>()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException(typeof(TModel), id);

        return entity;
    }

    public async Task DeletedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<TModel>()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException(typeof(TModel), id);

        _dbContext.Set<TModel>().Remove(entity);
    }
}

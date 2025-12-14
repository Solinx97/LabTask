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

    public async Task AddAsync(TModel item, CancellationToken ct = default)
    {
        await _dbContext.Set<TModel>().AddAsync(item, ct);
    }

    public async Task<TModel> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Set<TModel>()
            .FirstOrDefaultAsync(d => d.Id == id, ct)
                ?? throw new EntityNotFoundException(typeof(TModel), id);

        return entity;
    }

    public async Task DeletedAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Set<TModel>()
            .FirstOrDefaultAsync(d => d.Id == id, ct)
                ?? throw new EntityNotFoundException(typeof(TModel), id);

        _dbContext.Set<TModel>().Remove(entity);
    }
}

using LabTask.Domain.Data;
using LabTask.Infrastructure.Persistence;

namespace LabTask.Infrastructure.Data;

internal class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}

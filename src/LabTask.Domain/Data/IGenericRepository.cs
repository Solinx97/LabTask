namespace LabTask.Domain.Data;

public interface IGenericRepository<TModel>
    where TModel : class
{
    Task AddAsync(TModel item);

    Task<TModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task DeletedAsync(Guid id, CancellationToken cancellationToken = default);
}

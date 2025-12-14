namespace LabTask.Domain.Data;

public interface IGenericRepository<TModel>
    where TModel : class
{
    Task AddAsync(TModel item, CancellationToken ct = default);

    Task<TModel> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task DeletedAsync(Guid id, CancellationToken ct = default);
}

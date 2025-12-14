using LabTask.Domain.Entities;

namespace LabTask.Domain.Data;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByDocumentIdAsync(Guid documentId, int page, int pageSize, CancellationToken ct = default);
}

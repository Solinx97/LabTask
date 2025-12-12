using LabTask.Domain.Aggregates;

namespace LabTask.Domain.Data;

public interface IDocumentRepository : IGenericRepository<Document>
{
    Task<Document> GetByIdAsync(Guid id, Guid commentId, CancellationToken cancellationToken = default);
}

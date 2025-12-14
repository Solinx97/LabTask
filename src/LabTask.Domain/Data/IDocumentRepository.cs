using LabTask.Domain.Aggregates;
using LabTask.Domain.Helpers;
using System.Threading.Tasks;

namespace LabTask.Domain.Data;

public interface IDocumentRepository : IGenericRepository<Document>
{
    Task<Document> GetByIdAsync(Guid id, Guid commentId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Statistic>> GetStatisticsAsync(Guid userId, DateTimeOffset startedAt, DateTimeOffset finishedAt, CancellationToken ct = default);

    Task<IEnumerable<Statistic>> GetStatisticsAsync(Guid userId, CancellationToken ct = default);

    Task<IEnumerable<Document>> GetActualDocumentsAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);

    Task<IEnumerable<Document>> GetExpiredDocumentsAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);

    Task<IEnumerable<Document>> GetDocumentByNameAsync(Guid userId, string name, CancellationToken ct = default);
}

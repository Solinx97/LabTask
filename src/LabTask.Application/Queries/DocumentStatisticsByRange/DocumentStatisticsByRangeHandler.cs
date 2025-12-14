using LabTask.Domain.Data;
using LabTask.Domain.Helpers;
using MediatR;

namespace LabTask.Application.Queries.DocumentStatisticsByRange;

internal class DocumentStatisticsByRangeHandler(IDocumentRepository repository) : IRequestHandler<DocumentStatisticsByRangeQuery, IEnumerable<Statistic>>
{
    private readonly IDocumentRepository _repository = repository;

    public async Task<IEnumerable<Statistic>> Handle(DocumentStatisticsByRangeQuery request, CancellationToken ct)
    {
        var statistics = await _repository.GetStatisticsAsync(request.UserId, request.StartedAt, request.FinishedAt, ct);

        return statistics;
    }
}
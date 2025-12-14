using LabTask.Domain.Data;
using LabTask.Domain.Helpers;
using MediatR;

namespace LabTask.Application.Queries.DocumentStatisticsByYear;

internal class DocumentStatisticsByYearHandler(IDocumentRepository repository) : IRequestHandler<DocumentStatisticsByYearQuery, IEnumerable<Statistic>>
{
    private readonly IDocumentRepository _repository = repository;

    public async Task<IEnumerable<Statistic>> Handle(DocumentStatisticsByYearQuery request, CancellationToken ct)
    {
        var statistics = await _repository.GetStatisticsAsync(request.UserId, ct);

        return statistics;
    }
}
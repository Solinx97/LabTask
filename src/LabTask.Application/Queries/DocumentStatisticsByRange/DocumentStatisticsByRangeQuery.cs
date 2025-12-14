using LabTask.Domain.Helpers;
using MediatR;

namespace LabTask.Application.Queries.DocumentStatisticsByRange;

public record DocumentStatisticsByRangeQuery(
    Guid UserId,
    DateTimeOffset StartedAt,
    DateTimeOffset FinishedAt
    ) : IRequest<IEnumerable<Statistic>>;
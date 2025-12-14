using LabTask.Domain.Helpers;
using MediatR;

namespace LabTask.Application.Queries.DocumentStatisticsByYear;

public record DocumentStatisticsByYearQuery(
    Guid UserId
    ) : IRequest<IEnumerable<Statistic>>;
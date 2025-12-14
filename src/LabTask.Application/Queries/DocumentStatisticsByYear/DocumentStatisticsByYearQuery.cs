using LabTask.Application.Helper;
using MediatR;

namespace LabTask.Application.Queries.DocumentStatisticsByYear;

public record DocumentStatisticsByYearQuery(
    Guid UserId
    ) : IRequest<IEnumerable<StatisticByYear>>;
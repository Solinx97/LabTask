using LabTask.Application.Helper;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.DocumentStatisticsByYear;

internal class DocumentStatisticsByYearHandler(AppDbContext db) : IRequestHandler<DocumentStatisticsByYearQuery, IEnumerable<StatisticByYear>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<StatisticByYear>> Handle(DocumentStatisticsByYearQuery request, CancellationToken ct)
    {
        var statistic = await _db.Document
            .Where(doc => doc.UserId == request.UserId)
            .GroupBy(doc => doc.CreatedAt.Year)
            .Select(g => new StatisticByYear
            {
                Year = g.Key,
                CreatedAtCount = g.Count(),
                UpdatedAtCount = g.Count(x => x.UpdatedAt != null && x.UpdatedAt.Value.Year == g.Key),
                ExpiredAtCount = g.Count(x => x.ExpireAt.Year == g.Key),
            })
            .OrderBy(s => s.Year)
            .ToListAsync(ct);

        return statistic;
    }
}
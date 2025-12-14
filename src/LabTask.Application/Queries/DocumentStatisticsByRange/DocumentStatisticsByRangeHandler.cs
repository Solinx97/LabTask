using LabTask.Application.Helper;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.DocumentStatisticsByRange;

internal class DocumentStatisticsByRangeHandler(AppDbContext db) : IRequestHandler<DocumentStatisticsByRangeQuery, IEnumerable<StatisticByYear>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<StatisticByYear>> Handle(DocumentStatisticsByRangeQuery request, CancellationToken ct)
    {
        var statistic = await _db.Document
            .Where(doc => doc.UserId == request.UserId 
                    && (doc.CreatedAt >= request.StartedAt && doc.CreatedAt <= request.FinishedAt))
            .GroupBy(doc => doc.CreatedAt.Year)
            .Select(g => new StatisticByYear
            {
                Year = g.Key,
                CreatedAtCount = g.Count(),
                UpdatedAtCount = g.Count(x => x.UpdatedAt != null && (x.UpdatedAt.Value >= request.StartedAt && x.UpdatedAt.Value <= request.FinishedAt)),
                ExpiredAtCount = g.Count(x => x.ExpireAt >= request.StartedAt && x.ExpireAt <= request.FinishedAt),
            })
            .OrderBy(s => s.Year)
            .ToListAsync(ct);

        return statistic;
    }
}
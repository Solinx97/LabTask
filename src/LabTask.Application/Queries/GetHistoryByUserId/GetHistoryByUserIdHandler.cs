using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetHistoryByUserId;

internal class GetHistoryByUserIdHandler(AppDbContext db) : IRequestHandler<GetHistoryByUserIdQuery, IEnumerable<DocumentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<DocumentDto>> Handle(GetHistoryByUserIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var documents = await _db.Document
            .Where(o => o.UserId == request.UserId && o.ExpireAt < now)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return documents;
    }
}

using LabTask.Application.DTOs;
using LabTask.Domain.Aggregates;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetActualDocumentsByUserId;

public class GetActualDocumentsByUserIdHandler(AppDbContext db) : IRequestHandler<GetActualDocumentsByUserIdQuery, IEnumerable<DocumentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<DocumentDto>> Handle(GetActualDocumentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var documents = await _db.Set<Document>()
            .Where(o => o.UserId == request.UserId && o.ExpireAt >= now)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return documents;
    }
}

using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetDocumentByName;

public class GetDocumentByNameHandler(AppDbContext db) : IRequestHandler<GetDocumentByNameQuery, IEnumerable<DocumentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentByNameQuery request, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;

        var documents = await _db.Document
            .Where(o => o.Name.StartsWith(request.Name) && o.ExpireAt >= now)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .ToListAsync(ct);

        return documents;
    }
}

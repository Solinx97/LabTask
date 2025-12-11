using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetDocument;

public class GetDocumentsByUserIdHandler(AppDbContext db) : IRequestHandler<GetDocumentsByUserIdQuery, IEnumerable<DocumentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var documents = await _db.Document
            .Where(o => o.UserId == request.UserId)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .ToListAsync();

        return documents;
    }
}

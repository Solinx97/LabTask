using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetDocument;

public class GetDocumentHandler(AppDbContext db) : IRequestHandler<GetDocumentQuery, DocumentDto>
{
    private readonly AppDbContext _db = db;

    public async Task<DocumentDto> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var document = await _db.Document
            .Where(o => o.Id == request.Id)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .FirstOrDefaultAsync();

        return document;
    }
}

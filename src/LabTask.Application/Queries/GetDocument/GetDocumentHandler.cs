using LabTask.Application.DTOs;
using LabTask.Domain.Exceptions;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetDocument;

public class GetDocumentHandler(AppDbContext db) : IRequestHandler<GetDocumentQuery, DocumentDto>
{
    private readonly AppDbContext _db = db;

    public async Task<DocumentDto> Handle(GetDocumentQuery request, CancellationToken ct)
    {
        var document = await _db.Document
            .Where(o => o.Id == request.Id)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .FirstOrDefaultAsync(ct)
                ?? throw new DomainException($"Document {request.Id} not found");

        return document;
    }
}

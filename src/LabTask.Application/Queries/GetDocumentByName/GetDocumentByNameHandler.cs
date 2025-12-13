using LabTask.Application.DTOs;
using LabTask.Domain.Exceptions;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetDocumentByName;

public class GetDocumentByNameHandler(AppDbContext db) : IRequestHandler<GetDocumentByNameQuery, DocumentDto>
{
    private readonly AppDbContext _db = db;

    public async Task<DocumentDto> Handle(GetDocumentByNameQuery request, CancellationToken ct)
    {
        var document = await _db.Document
            .Where(o => o.Name == request.Name)
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .FirstOrDefaultAsync(ct)
                ?? throw new DomainException($"Document {request.Name} not found");

        return document;
    }
}

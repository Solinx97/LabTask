using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetAllDocuments;

public class GetAllDocumentsHandler(AppDbContext db) : IRequestHandler<GetAllDocumentsQuery, IEnumerable<DocumentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<DocumentDto>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        var document = await _db.Document
            .Select(o => new DocumentDto(o.Id, o.Name, o.Description, o.ExpireAt, o.UserId))
            .ToListAsync();

        return document;
    }
}

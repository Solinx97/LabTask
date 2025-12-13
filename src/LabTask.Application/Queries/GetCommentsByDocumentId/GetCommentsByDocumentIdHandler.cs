using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetCommentsByDocumentId;

internal class GetCommentsByDocumentIdHandler(AppDbContext db) : IRequestHandler<GetCommentsByDocumentIdQuery, IEnumerable<CommentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByDocumentIdQuery request, CancellationToken ct)
    {
        var comments = await _db.Document
            .Where(d => d.Id == request.DocumentId)
            .SelectMany(d => d.Comments)
            .Select(c => new CommentDto(c.Id, c.Content, c.DocumentId, c.UserId))
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return comments;
    }
}

using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetCommentsByUserId;

public class GetCommentsByUserIdHandler(AppDbContext db) : IRequestHandler<GetCommentsByUserIdQuery, IEnumerable<CommentDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var comments = await _db.Document
            .Where(o => o.UserId == request.UserId)
            .SelectMany(d => d.Comments)
            .Select(o => new CommentDto(o.Id, o.Content, o.DocumentId, o.UserId))
            .ToListAsync(cancellationToken);

        return comments;
    }
}

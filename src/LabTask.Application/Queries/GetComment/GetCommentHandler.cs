using LabTask.Application.DTOs;
using LabTask.Domain.Exceptions;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetComment;

public class GetCommentHandler(AppDbContext db) : IRequestHandler<GetCommentQuery, CommentDto>
{
    private readonly AppDbContext _db = db;

    public async Task<CommentDto> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var comment = await _db.Comment
            .Where(o => o.Id == request.Id)
            .Select(o => new CommentDto(o.Id, o.Content, o.DocumentId, o.UserId))
            .FirstOrDefaultAsync()
                ?? throw new DomainException($"Comment {request.Id} not found");

        return comment;
    }
}

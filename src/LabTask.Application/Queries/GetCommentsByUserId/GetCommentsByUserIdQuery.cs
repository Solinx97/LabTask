using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetCommentsByUserId;

public record GetCommentsByUserIdQuery(
    Guid UserId
    ) : IRequest<IEnumerable<CommentDto>>;

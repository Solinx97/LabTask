using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetComment;

public record GetCommentQuery(
    Guid Id,
    Guid DocumentId
    ) : IRequest<CommentDto>;

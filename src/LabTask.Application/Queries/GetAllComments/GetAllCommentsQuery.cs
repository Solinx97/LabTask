using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetAllComments;

public record GetAllCommentsQuery(
    Guid DocumentId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommentDto>>;

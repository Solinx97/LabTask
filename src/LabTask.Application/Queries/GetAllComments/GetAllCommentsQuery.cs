using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetAllComments;

public record GetAllCommentsQuery(
    Guid DocumentId
    ) : IRequest<IEnumerable<CommentDto>>;

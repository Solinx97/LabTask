using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetCommentsByDocumentId;

public record GetCommentsByDocumentIdQuery(
    Guid DocumentId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommentDto>>;

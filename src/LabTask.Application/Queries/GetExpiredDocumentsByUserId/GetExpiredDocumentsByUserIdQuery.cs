using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetExpiredDocumentsByUserId;

public record GetExpiredDocumentsByUserIdQuery(
    Guid UserId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DocumentDto>>;
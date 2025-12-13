using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetActualDocumentsByUserId;

public record GetActualDocumentsByUserIdQuery(
    Guid UserId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DocumentDto>>;

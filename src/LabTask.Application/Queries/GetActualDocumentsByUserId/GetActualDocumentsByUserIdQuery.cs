using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetActualDocumentsByUserId;

public record GetActualDocumentsByUserIdQuery(
    Guid UserId
    ) : IRequest<IEnumerable<DocumentDto>>;

using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetDocumentsByUserId;

public record GetDocumentsByUserIdQuery(Guid UserId) : IRequest<IEnumerable<DocumentDto>>;

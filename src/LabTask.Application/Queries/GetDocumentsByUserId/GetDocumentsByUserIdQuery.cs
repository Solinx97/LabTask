using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetDocument;

public record GetDocumentsByUserIdQuery(Guid UserId) : IRequest<IEnumerable<DocumentDto>>;

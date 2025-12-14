using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetDocumentsByName;

public record GetDocumentsByNameQuery(
    Guid UserId,
    string Name
    ) : IRequest<IEnumerable<DocumentDto>>;

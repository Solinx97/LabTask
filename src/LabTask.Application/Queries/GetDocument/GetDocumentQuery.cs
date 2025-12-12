using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetDocument;

public record GetDocumentQuery(
    string Name
    ) : IRequest<DocumentDto>;

using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetDocument;

public record GetDocumentQuery(Guid Id) : IRequest<DocumentDto>;

using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetAllDocuments;

public record GetAllDocumentsQuery() : IRequest<IEnumerable<DocumentDto>>;

using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetDocumentByName;

public record GetDocumentByNameQuery(
    string Name
    ) : IRequest<DocumentDto>;

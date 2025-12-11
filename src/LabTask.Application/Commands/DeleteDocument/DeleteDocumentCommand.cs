using MediatR;

namespace LabTask.Application.Commands.DeleteDocument;

public record DeleteDocumentCommand(
    Guid Id
    ) : IRequest;

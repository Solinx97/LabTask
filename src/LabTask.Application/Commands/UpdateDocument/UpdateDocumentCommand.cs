using MediatR;

namespace LabTask.Application.Commands.UpdateDocument;

public record UpdateDocumentCommand(
    Guid Id, 
    string Name, 
    string Description
    ) : IRequest;

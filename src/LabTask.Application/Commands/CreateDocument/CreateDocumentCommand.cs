using MediatR;

namespace LabTask.Application.Commands.CreateDocument;

public record CreateDocumentCommand(
    string Name,
    string Description, 
    DateTimeOffset ExpireAt,
    Guid UserId
    ) : IRequest<Guid>;

using MediatR;

namespace LabTask.Application.Commands.CreateDocument;

public record CreateDocumentCommand(string Name, string Description, DateTimeOffset ExpireAt) : IRequest<Guid>;

using MediatR;

namespace LabTask.Application.Commands.DeleteLink;

public record DeleteLinkCommand(
    Guid Id,
    Guid DocumentId
    ) : IRequest;

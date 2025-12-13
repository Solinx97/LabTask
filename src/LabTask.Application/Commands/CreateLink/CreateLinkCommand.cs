using MediatR;

namespace LabTask.Application.Commands.CreateLink;

public record CreateLinkCommand(
    Guid DocumentId,
    Guid OwnerId,
    Guid ToUserId,
    DateTimeOffset ExpireAt
    ) : IRequest<Guid>;

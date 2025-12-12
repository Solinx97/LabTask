using MediatR;

namespace LabTask.Application.Commands.DeleteComment;

public record DeleteCommentCommand(
    Guid Id,
    Guid DocumentId
    ) : IRequest;
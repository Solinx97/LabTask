using MediatR;

namespace LabTask.Application.Commands.UpdateComment;

public record UpdateCommentCommand(
    Guid Id,
    string Content,
    Guid DocumentId
    ) : IRequest;

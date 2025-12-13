using LabTask.Domain.Entities;
using MediatR;

namespace LabTask.Application.Commands.CreateComment;

public record CreateCommentCommand(
    string Content,
    Guid DocumentId,
    Guid UserId
    ) : IRequest<Comment>;

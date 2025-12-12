namespace LabTask.Application.DTOs;

public record CommentDto(
    Guid Id,
    string Content,
    Guid DocumentId,
    Guid UserId
    );

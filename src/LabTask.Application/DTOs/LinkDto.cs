namespace LabTask.Application.DTOs;

public record LinkDto(
    Guid Id,
    Guid Uri,
    Guid DocumentId,
    Guid OwnerId,
    Guid ToUserId,
    DateTimeOffset ExpireAt
    );

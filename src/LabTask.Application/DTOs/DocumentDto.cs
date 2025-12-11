namespace LabTask.Application.DTOs;

public record DocumentDto(Guid Id, string Name, string Description, DateTimeOffset ExpireAt);

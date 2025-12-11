using LabTask.Domain.Enums;

namespace LabTask.Domain.Exceptions;

public class DomainException(string message, ExceptionCode code = ExceptionCode.DomainError) : Exception(message)
{
    public ExceptionCode Code { get; } = code;

    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

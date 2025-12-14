namespace LabTask.Domain.Exceptions;

public class DocumentExpireAtExcepction(string message) : DomainException(message)
{
    public string Message { get; } = message;

    public static void ThrowIfPastTime(DateTimeOffset expireAt)
    {
        var now = DateTimeOffset.UtcNow;
        if (expireAt < now)
        {
            throw new DocumentExpireAtExcepction("Document expire at should be more than right now time.");
        }
    }
}

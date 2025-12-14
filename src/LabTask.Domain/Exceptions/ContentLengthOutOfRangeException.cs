namespace LabTask.Domain.Exceptions;

public class ContentLengthOutOfRangeException(string message) : DomainException(message)
{
    public string Message { get; } = message;

    public static void ThrowIfLong(string content, int maxLength, string contentName)
    {
        if (content.Length > maxLength)
        {
            throw new ContentLengthOutOfRangeException($"{contentName} length is too long.");
        }
    }
}

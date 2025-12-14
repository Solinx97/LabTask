using LabTask.Domain.Aggregates;
using LabTask.Domain.Exceptions;
using LabTask.Domain.Interfaces;

namespace LabTask.Domain.Entities;

public class Comment : IEntityId
{
    public const int CONTENT_MAX_LENGTH = 500;

    private Comment() { }

    private Comment(string content, Guid documentId, Guid userId)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        Content = content;
        DocumentId = documentId;
        UserId = userId;
    }

    public Guid Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public Guid DocumentId { get; private set; }

    public Guid UserId { get; private set; }

    public Document Document { get; private set; } = null!;

    public static Comment Create(string content, Guid documentId, Guid userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentNullException.ThrowIfNull(documentId, nameof(documentId));
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));

        ContentLengthOutOfRangeException.ThrowIfLong(content, CONTENT_MAX_LENGTH, nameof(Content));

        return new Comment(content, documentId, userId);
    }

    public void Edit(string content)
    {
        if (!string.IsNullOrEmpty(content) 
            && !string.Equals(content, Content, StringComparison.OrdinalIgnoreCase)
            && content.Length <= CONTENT_MAX_LENGTH)
        {
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

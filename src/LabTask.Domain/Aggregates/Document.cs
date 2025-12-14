using LabTask.Domain.Entities;
using LabTask.Domain.Exceptions;
using LabTask.Domain.Interfaces;

namespace LabTask.Domain.Aggregates;

public class Document : IEntityId
{
    private readonly List<Comment> _comments = [];

    public const int NAME_MAX_LENGTH = 128;

    private Document() {}

    private Document(string name, string description, DateTimeOffset expireAt, Guid userId)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        Name = name;
        Description = description;
        ExpireAt = expireAt;
        UserId = userId;
    }

    public Guid Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public DateTimeOffset ExpireAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public Guid UserId { get; private set; }

    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    public static Document Create(string name, string description, DateTimeOffset expireAt, Guid userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentNullException.ThrowIfNull(description, nameof(description));
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));

        ContentLengthOutOfRangeException.ThrowIfLong(name, NAME_MAX_LENGTH, nameof(Name));
        DocumentExpireAtExcepction.ThrowIfPastTime(expireAt);

        return new Document(name, description, expireAt, userId);
    }

    public void AddComment(string content, Guid userId)
    {
        var comment = Comment.Create(content, Id, userId);
        _comments.Add(comment);
    }

    public void DeleteComment(Guid commentId)
    {
        _comments.RemoveAll(c => c.Id == commentId);
    }

    public void EditComment(string content, Guid commentId)
    {
        var comment = _comments.SingleOrDefault(c => c.Id == commentId);
        if (comment != null)
        {
            comment.Edit(content);
        }
        else
        {
            throw new DomainException("Comment not found");
        }
    }

    public void Edit(string name, string description)
    {
        DocumentExpireAtExcepction.ThrowIfPastTime(ExpireAt);

        if (!string.IsNullOrEmpty(name)
            && !string.Equals(name, Name, StringComparison.OrdinalIgnoreCase)
            && name.Length <= NAME_MAX_LENGTH)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        if (!string.IsNullOrEmpty(description) 
            && !string.Equals(description, Description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

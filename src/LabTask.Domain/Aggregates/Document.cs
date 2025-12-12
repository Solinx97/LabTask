using LabTask.Domain.Entities;
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
        Name = name;
        Description = description;
        ExpireAt = expireAt;
        UserId = userId;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public DateTimeOffset ExpireAt { get; private set; }

    public Guid UserId { get; private set; }

    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    public static Document Create(string name, string description, DateTimeOffset expireAt, Guid userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NAME_MAX_LENGTH, nameof(name));
        ArgumentNullException.ThrowIfNull(description, nameof(description));
        ArgumentOutOfRangeException.ThrowIfLessThan(expireAt, DateTimeOffset.UtcNow, nameof(expireAt));
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));

        return new Document(name, description, expireAt, userId);
    }

    public void Edit(string name, string description)
    {
        if (!string.IsNullOrEmpty(name) && !string.Equals(name, Name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
        }

        if (!string.IsNullOrEmpty(description) && !string.Equals(description, Description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
        }
    }
}

namespace LabTask.Domain.Aggregates;

public class Document
{
    public const int NAME_MAX_LENGTH = 128;

    private Document() {}

    private Document(string name, string description, DateTimeOffset expireAt)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ExpireAt = expireAt;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTimeOffset ExpireAt { get; set; }

    public static Document Create(string name, string description, DateTimeOffset expireAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NAME_MAX_LENGTH, nameof(name));
        ArgumentNullException.ThrowIfNull(description, nameof(description));
        ArgumentOutOfRangeException.ThrowIfLessThan(expireAt, DateTimeOffset.UtcNow, nameof(expireAt));

        return new Document(name, description, expireAt);
    }
}

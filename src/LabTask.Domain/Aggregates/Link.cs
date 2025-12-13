using LabTask.Domain.Exceptions;
using LabTask.Domain.Interfaces;

namespace LabTask.Domain.Aggregates;

public class Link : IEntityId
{
    private Link() { }

    private Link(Guid documentId, Guid ownerId, Guid toUserId, DateTimeOffset expireAt)
    {
        Id = Guid.NewGuid();
        Uri = Guid.NewGuid();
        DocumentId = documentId;
        OwnerId = ownerId;
        ToUserId = toUserId;
        ExpireAt = expireAt;
    }

    public Guid Id { get; private set; }

    public Guid Uri { get; private set; }

    public Guid DocumentId { get; private set; }

    public Guid OwnerId { get; private set; }

    public Guid ToUserId { get; private set; }

    public DateTimeOffset ExpireAt { get; private set; }

    public static Link Create(Guid documentId, Guid ownerId, Guid toUserId, DateTimeOffset expireAt)
    {
        ArgumentNullException.ThrowIfNull(documentId, nameof(documentId));
        ArgumentNullException.ThrowIfNull(ownerId, nameof(ownerId));
        ArgumentNullException.ThrowIfNull(toUserId, nameof(toUserId));

        DocumentExpireAtExcepction.ThrowIfPastTime(expireAt);

        return new Link(documentId, ownerId, toUserId, expireAt);
    }
}

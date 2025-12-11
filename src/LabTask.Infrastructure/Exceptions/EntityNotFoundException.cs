using LabTask.Domain.Enums;
using LabTask.Domain.Exceptions;

namespace LabTask.Infrastructure.Exceptions;

public class EntityNotFoundException(Type entityType, object entityId) : DomainException($"Entity '{entityType.Name}' with Id '{entityId}' was not found.", ExceptionCode.NotFound)
{
    public Type EntityType { get; } = entityType;

    public object EntityId { get; } = entityId;
}

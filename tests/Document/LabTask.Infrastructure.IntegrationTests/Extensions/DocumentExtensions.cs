using LabTask.Domain.Aggregates;

namespace LabTask.Infrastructure.IntegrationTests.Extensions;

internal static class DocumentExtensions
{
    public static void SetExpireAt(this Document doc, DateTimeOffset expireAt)
    {
        doc.GetType().GetProperty(nameof(Document.ExpireAt)).SetValue(doc, expireAt);
    }
}

using LabTask.Domain.Aggregates;
using LabTask.Infrastructure.Data;
using LabTask.Infrastructure.IntegrationTests.Data;

namespace LabTask.Infrastructure.IntegrationTests.RepositoryTests;

[Collection("SQL Server Tests")]
public class LinkRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByOwnerIdAsync_Links_GetLinksByOwnerId()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var ownerId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("6311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var page = 0;
        var pagSize = 10;

        var repo = new LinkRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, ownerId),
            Document.Create("test name 2", "test description 2", expires2At, ownerId),
            Document.Create("test name 3", "test description 3", expires3At, ownerId),
        };

        await context.Document.AddRangeAsync(docs);

        var links = new List<Link>
        {
            Link.Create(docs[0].Id, ownerId, userId, expires1At),
            Link.Create(docs[0].Id, ownerId, userId, expires2At),
            Link.Create(docs[0].Id, ownerId, userId, expires3At),
        };

        await context.Link.AddRangeAsync(links);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByOwnerIdAsync(ownerId, docs[0].Id, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(links.Count, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByUserIdAsync_Links_GetLinksByUserId()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var ownerId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("6311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var page = 0;
        var pagSize = 10;

        var repo = new LinkRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, ownerId),
            Document.Create("test name 2", "test description 2", expires2At, ownerId),
            Document.Create("test name 3", "test description 3", expires3At, ownerId),
        };

        await context.Document.AddRangeAsync(docs);

        var links = new List<Link>
        {
            Link.Create(docs[0].Id, ownerId, userId, expires1At),
            Link.Create(docs[0].Id, ownerId, userId, expires2At),
            Link.Create(docs[0].Id, ownerId, userId, expires3At),
        };

        await context.Link.AddRangeAsync(links);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByUserIdAsync(userId, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(links.Count, result.Count());

        await transaction.RollbackAsync();
    }
}

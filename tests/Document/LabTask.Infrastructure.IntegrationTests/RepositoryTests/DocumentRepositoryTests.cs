using LabTask.Domain.Aggregates;
using LabTask.Infrastructure.Data;
using LabTask.Infrastructure.Exceptions;
using LabTask.Infrastructure.IntegrationTests.Data;
using LabTask.Infrastructure.IntegrationTests.Extensions;

namespace LabTask.Infrastructure.IntegrationTests.RepositoryTests;

[CollectionDefinition("SQL Server Tests")]
public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture> { }

[Collection("SQL Server Tests")]
public class DocumentRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByIdAsync_Document_ShouldCreateEntityAndReturnCreatedEntity()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var commentId = Guid.Parse("8d724186-bcb2-4bb5-a687-63987d58b51b");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new DocumentRepository(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByIdAsync(doc.Id, commentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(doc.Id, result.Id);
        Assert.Single(context.Document);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByIdAsync_ThrowEntityNotFoundException_ShouldNootGetDocumentAndThrowException()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var id = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var commentId = Guid.Parse("8d724186-bcb2-4bb5-a687-63987d58b51b");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new DocumentRepository(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repo.GetByIdAsync(id, commentId));

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetStatisticsAsync_Statistics_ShouldReturnUserStatistics()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new DocumentRepository(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetStatisticsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetStatisticsAsync_Statistics_ShouldReturnUserStatisticsByRange()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var startedAt = DateTimeOffset.Parse("8/20/2024 7:17:46 AM +00:00");
        var finishedAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new DocumentRepository(context);
        var docs = new List<Document>
        { 
            Document.Create("test name", "test description", expires1At, userId),
            Document.Create("test name 2", "test description 2", expires2At, userId),
            Document.Create("test name 3", "test description 3", expires3At, userId),
        };

        await context.Document.AddRangeAsync(docs);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetStatisticsAsync(userId, startedAt, finishedAt);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetActualDocumentsAsync_Documents_ShouldReturnOnlyActualDocuments()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var page = 0;
        var pagSize = 10;

        var repo = new DocumentRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, userId),
            Document.Create("test name 2", "test description 2", expires2At, userId),
            Document.Create("test name 3", "test description 3", expires3At, userId),
        };

        await context.Document.AddRangeAsync(docs);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetActualDocumentsAsync(userId, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(docs.Count, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetActualDocumentsAsync_Documents_ShouldReturnNoAnyDocument()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");

        var page = 0;
        var pagSize = 10;

        var repo = new DocumentRepository(context);

        // Act
        var result = await repo.GetActualDocumentsAsync(userId, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetExpiredDocumentsAsync_Documents_ShouldReturnOnlyExpiredDocuments()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var page = 0;
        var pagSize = 10;

        var repo = new DocumentRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, userId),
            Document.Create("test name 2", "test description 2", expires2At, userId),
            Document.Create("test name 3", "test description 3", expires3At, userId),
        };

        // Use reflection to create expired Documents
        var expiredTime = DateTimeOffset.Parse("6/20/2023 7:17:46 AM +00:00");
        foreach (var item in docs)
        {
            item.SetExpireAt(expiredTime);
        }

        await context.Document.AddRangeAsync(docs);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetExpiredDocumentsAsync(userId, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(docs.Count, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetExpiredDocumentsAsync_Documents_ShouldReturnNoAnyDocuments()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");

        var page = 0;
        var pagSize = 10;

        var repo = new DocumentRepository(context);

        // Act
        var result = await repo.GetExpiredDocumentsAsync(userId, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetDocumentByNameAsync_Documents_ShouldReturnDocumentsByName()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var name = "test";

        var repo = new DocumentRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, userId),
            Document.Create("test name 2", "test description 2", expires2At, userId),
            Document.Create("test name 3", "test description 3", expires3At, userId),
        };

        await context.Document.AddRangeAsync(docs);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetDocumentByNameAsync(userId, name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(docs.Count, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetDocumentByNameAsync_Documents_ShouldReturnNoAnyDocuments()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expires1At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires2At = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");
        var expires3At = DateTimeOffset.Parse("6/20/2027 7:17:46 AM +00:00");

        var name = "check";

        var repo = new DocumentRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, userId),
            Document.Create("test name 2", "test description 2", expires2At, userId),
            Document.Create("test name 3", "test description 3", expires3At, userId),
        };

        await context.Document.AddRangeAsync(docs);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetDocumentByNameAsync(userId, name);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await transaction.RollbackAsync();
    }
}

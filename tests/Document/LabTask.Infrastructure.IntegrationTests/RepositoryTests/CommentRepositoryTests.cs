using LabTask.Domain.Aggregates;
using LabTask.Domain.Entities;
using LabTask.Infrastructure.Data;
using LabTask.Infrastructure.IntegrationTests.Data;

namespace LabTask.Infrastructure.IntegrationTests.RepositoryTests;

[Collection("SQL Server Tests")]
public class CommentRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByIdAsync_Document_ShouldCreateEntityAndReturnCreatedEntity()
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

        var repo = new CommentRepository(context);
        var docs = new List<Document>
        {
            Document.Create("test name", "test description", expires1At, userId),
            Document.Create("test name 2", "test description 2", expires2At, userId),
            Document.Create("test name 3", "test description 3", expires3At, userId),
        };

        await context.Document.AddRangeAsync(docs);

        var comments = new List<Comment>
        {
            Comment.Create("test name", docs[0].Id, userId),
            Comment.Create("test name 2", docs[0].Id, userId),
            Comment.Create("test name 3", docs[0].Id, userId),
        };

        await context.Comment.AddRangeAsync(comments);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByDocumentIdAsync(docs[0].Id, page, pagSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(comments.Count, result.Count());

        await transaction.RollbackAsync();
    }
}

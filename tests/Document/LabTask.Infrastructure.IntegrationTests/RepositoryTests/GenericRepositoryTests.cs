using LabTask.Domain.Aggregates;
using LabTask.Infrastructure.Data;
using LabTask.Infrastructure.Exceptions;
using LabTask.Infrastructure.IntegrationTests.Data;

namespace LabTask.Infrastructure.IntegrationTests.RepositoryTests;

[Collection("SQL Server Tests")]
public class GenericRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByIdAsync_Document_ShouldReturnDocumentById()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new GenericRepository<Document>(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByIdAsync(doc.Id);

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
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new GenericRepository<Document>(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repo.GetByIdAsync(id));

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task DeletedAsync_ShouldDeleteExistEntity()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new GenericRepository<Document>(context);
        var uow = new UnitOfWork(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act
        await repo.DeletedAsync(doc.Id);
        await uow.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Document);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task DeletedAsync_ThrowEntityNotFoundException_ShouldNotDeleteExistEntityAndThrowException()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var id = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var repo = new GenericRepository<Document>(context);
        var uow = new UnitOfWork(context);
        var doc = Document.Create("test name", "test description", expiresAt, userId);

        await context.Document.AddAsync(doc);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => repo.DeletedAsync(id));

        await transaction.RollbackAsync();
    }
}

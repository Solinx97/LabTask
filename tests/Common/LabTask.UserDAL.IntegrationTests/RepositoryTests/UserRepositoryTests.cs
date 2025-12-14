using LabTask.UserDAL.Entities;
using LabTask.UserDAL.IntegrationTests.IntegrationTests.Data;
using LabTask.UserDAL.Repositories;

namespace LabTask.UserDAL.IntegrationTests.RepositoryTests;


[CollectionDefinition("SQL Server Tests")]
public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture> { }

[Collection("SQL Server Tests")]
public class UserRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetAllAsync_Users_ShouldReturnAllUsers()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var repo = new UserRepository(context);
        var user = new ApplicationUser
        {
            Id = "uid-2",
            Email = "test@yandex.by",
        };

        await context.ApplicationUser.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetAllAsync_Users_ShouldReturnNoAnyUsers()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var repo = new UserRepository(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByIdAsync_User_ShouldReturnUserById()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var userId = "uid-2";

        var repo = new UserRepository(context);
        var user = new ApplicationUser
        {
            Id = userId,
            Email = "test@yandex.by",
        };

        await context.ApplicationUser.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);

        await transaction.RollbackAsync();
    }
}

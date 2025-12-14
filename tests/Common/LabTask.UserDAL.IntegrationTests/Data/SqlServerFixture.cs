using LabTask.UserDAL.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace LabTask.UserDAL.IntegrationTests.IntegrationTests.Data;

public class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    public SqlServerFixture()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Oleg123*")
            .Build();
    }

    public DbContextOptions<UserContext> Options { get; private set; } = null!;

    public UserContext CreateContext()
    {
        return new UserContext(Options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var connectionString = _container.GetConnectionString();
        Options = new DbContextOptionsBuilder<UserContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}

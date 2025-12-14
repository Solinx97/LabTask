using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace LabTask.Infrastructure.IntegrationTests.Data;

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

    public DbContextOptions<AppDbContext> Options { get; private set; } = null!;

    public AppDbContext CreateContext()
    {
        return new AppDbContext(Options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var connectionString = _container.GetConnectionString();
        Options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}

using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using LabTask.Domain.Entities;
using LabTask.Infrastructure.Data;
using LabTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LabTask.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IGenericRepository<Document>, GenericRepository<Document>>();
        services.AddScoped<IGenericRepository<Comment>, GenericRepository<Comment>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}

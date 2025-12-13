using LabTask.UserDAL.Data;
using LabTask.UserDAL.Interfaces;
using LabTask.UserDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LabTask.UserDAL.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddUserData(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserContext>(options =>
        {
          options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
    }
}

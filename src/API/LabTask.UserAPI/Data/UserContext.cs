using LabTask.UserAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabTask.UserAPI.Data;

public class UserContext: DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<ApplicationUser> ApplicationUser { get; set; } = null!;
}

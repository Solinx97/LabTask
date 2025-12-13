using LabTask.UserDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabTask.UserDAL.Data;

public class UserContext: DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<ApplicationUser> ApplicationUser { get; set; } = null!;
}

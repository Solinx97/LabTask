using LabTask.UserDAL.Data;
using LabTask.UserDAL.Entities;
using LabTask.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabTask.UserDAL.Repositories;

internal class UserRepository(UserContext dbContext) : IUserRepository
{
    private readonly UserContext _dbContext = dbContext;

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _dbContext.ApplicationUser
            .ToListAsync(ct);

        return users;
    }

    public async Task<ApplicationUser> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var user = await _dbContext.ApplicationUser
            .SingleOrDefaultAsync(a => a.Id == id, ct);

        return user;
    }
}

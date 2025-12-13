using LabTask.UserAPI.Data;
using LabTask.UserAPI.Entities;
using LabTask.UserAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabTask.UserAPI.Repositories;

internal class UserRepository(UserContext dbContext) : IUserRepository
{
    private readonly UserContext _dbContext = dbContext;

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _dbContext.ApplicationUser
            .ToListAsync(cancellationToken);

        return users;
    }
}

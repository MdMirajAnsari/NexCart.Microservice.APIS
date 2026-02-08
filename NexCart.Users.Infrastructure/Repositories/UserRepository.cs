using Microsoft.EntityFrameworkCore;
using NexCart.Users.Entities;
using NexCart.Users.RepositoryContracts;

namespace NexCart.Users.Infrastructure.Repositories;

public class UserRepository : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();

        await _dbContext.Users.AddAsync(user);
        int saved = await _dbContext.SaveChangesAsync();
        return saved > 0 ? user : null;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        if (email == null || password == null) return null;
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    public async Task<ApplicationUser?> GetUserByUserId(Guid? userId)
    {
        if (userId == null) return null;
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }
}
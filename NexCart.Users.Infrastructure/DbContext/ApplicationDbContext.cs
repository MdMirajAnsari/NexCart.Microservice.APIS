using Microsoft.EntityFrameworkCore;
using NexCart.Users.Entities;

namespace NexCart.Users.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; } = null!;
}






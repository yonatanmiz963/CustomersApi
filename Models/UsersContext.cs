using Microsoft.EntityFrameworkCore;

namespace CustomersApi.Models;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
    }

    public DbSet<User> UserItems { get; set; } = null!;
}
using Microsoft.EntityFrameworkCore;
using Puush.Persistence.Models;

namespace Puush.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Upload> Uploads { get; set; }
}

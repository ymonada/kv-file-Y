
using kvFileY.Domain.Entities;
using kvFileY.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {}
    public DbSet<User> Users { get; set; }
    public DbSet<FileY> Files { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new FileYConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
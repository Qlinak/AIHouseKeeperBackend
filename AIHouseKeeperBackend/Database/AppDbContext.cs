using AIHouseKeeper.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace AIHouseKeeperBackend.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Memory> Memories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Memory)
            .WithOne()
            .HasForeignKey<Memory>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
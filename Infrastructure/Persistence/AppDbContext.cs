using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Extensions;

namespace SparkUpSolution.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Bonus> Bonuses => Set<Bonus>();
        // public DbSet<BonusAuditLog> BonusAuditLogs => Set<BonusAuditLog>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bonus>()
                .HasIndex(b => new { b.PlayerId, b.Type, b.Status });

            modelBuilder.Entity<Bonus>()
                .HasOne(b => b.Player)
                .WithMany(p => p.Bonuses)
                .HasForeignKey(b => b.PlayerId);
            
            base.OnModelCreating(modelBuilder);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
            {
                modelBuilder.Seed();
            }
        }
    }
}

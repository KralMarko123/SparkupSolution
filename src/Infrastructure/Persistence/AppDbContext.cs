using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Infrastructure.Logging;

namespace SparkUpSolution.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Bonus> Bonuses => Set<Bonus>();
        public DbSet<BonusAuditLog> BonusAuditLogs => Set<BonusAuditLog>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // properties
            modelBuilder.Entity<Player>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Bonus>()
                .Property(b => b.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<BonusAuditLog>()
                .Property(b => b.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // indices
            modelBuilder.Entity<Bonus>()
                .HasIndex(b => new { b.PlayerId, b.Type, b.Status });

            // relations
            modelBuilder.Entity<Bonus>()
                .HasOne(b => b.Player)
                .WithMany(p => p.Bonuses)
                .HasForeignKey(b => b.PlayerId);
        }
    }
}

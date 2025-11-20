using SparkUpSolution.Infrastructure.Logging;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public interface IBonusAuditRepository
    {
        Task AddAsync(BonusAuditLog log, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

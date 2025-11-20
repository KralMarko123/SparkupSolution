namespace SparkUpSolution.Infrastructure.Logging
{
    public interface IBonusAuditRepository
    {
        Task AddAsync(BonusAuditLog log, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

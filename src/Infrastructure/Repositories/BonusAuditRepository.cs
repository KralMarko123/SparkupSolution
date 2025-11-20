using SparkUpSolution.Infrastructure.Logging;
using SparkUpSolution.Infrastructure.Persistence;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public class BonusAuditRepository : IBonusAuditRepository
    {
        private readonly AppDbContext appDbContext;

        public BonusAuditRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(BonusAuditLog log, CancellationToken cancellationToken = default)
        {
            await appDbContext.BonusAuditLogs.AddAsync(log, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

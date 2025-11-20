using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;
using SparkUpSolution.Infrastructure.Persistence;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public class BonusRepository : IBonusRepository
    {
        private readonly AppDbContext appDbContext;

        public BonusRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PagedResult<Bonus>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = appDbContext.Bonuses;

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Bonus>(items, pageNumber, pageSize, total);
        }
        
        public async Task<Bonus?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await appDbContext.Bonuses.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<bool> HasActiveBonusOfTypeAsync(Guid playerId, BonusType type, Guid? exceptId = null, CancellationToken cancellationToken = default)
        {
            var query = appDbContext.Bonuses.Where(b => b.PlayerId == playerId && b.Type == type && b.Status == BonusStatus.Active);

            if (exceptId.HasValue)
            {
                query = query.Where(b => b.Id != exceptId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task AddAsync(Bonus bonus, CancellationToken cancellationToken)
        {
            await appDbContext.Bonuses.AddAsync(bonus, cancellationToken);
        }

        public async Task DeleteAsync(Bonus bonus, CancellationToken cancellationToken)
        {
            appDbContext.Bonuses.Remove(bonus);
            await Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Bonus bonus, CancellationToken cancellationToken)
        {
            appDbContext.Bonuses.Update(bonus);
            await Task.CompletedTask;
        }
    }
}

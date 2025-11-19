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

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Bonus>(items, pageNumber, pageSize, total);
        }

        Task IBonusRepository.AddAsync(Bonus bonus, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IBonusRepository.DeleteAsync(Bonus bonus, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Bonus?> IBonusRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBonusRepository.HasActiveBonusOfTypeAsync(Guid playerId, BonusType type, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<int> IBonusRepository.SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IBonusRepository.UpdateAsync(Bonus bonus, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

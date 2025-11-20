using SparkUpSolution.Application.Requests;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public interface IBonusRepository
    {
        Task<PagedResult<Bonus>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Bonus?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> HasActiveBonusOfTypeAsync(Guid playerId, BonusType type, Guid? exceptId, CancellationToken cancellationToken);
        Task AddAsync(Bonus bonus, CancellationToken cancellationToken);
        Task UpdateAsync(Bonus bonus, CancellationToken cancellationToken);
        Task DeleteAsync(Bonus bonus, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

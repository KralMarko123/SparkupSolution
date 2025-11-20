using SparkUpSolution.Application.Requests;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public interface IBonusRepository
    {
        Task<PagedResult<Bonus>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Bonus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> HasActiveBonusOfTypeAsync(Guid playerId, BonusType type, Guid? exceptId, CancellationToken cancellationToken = default);
        Task<Bonus> AddAsync(Bonus bonus, CancellationToken cancellationToken = default);
        Task UpdateAsync(Bonus bonus, CancellationToken cancellationToken = default);
        Task DeleteAsync(Bonus bonus, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

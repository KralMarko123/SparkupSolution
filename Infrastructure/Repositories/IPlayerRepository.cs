using SparkUpSolution.Domain.Entities;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}

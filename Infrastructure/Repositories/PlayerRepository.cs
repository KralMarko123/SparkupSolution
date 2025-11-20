using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Infrastructure.Persistence;

namespace SparkUpSolution.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext appDbContext;

        public PlayerRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await appDbContext.Players.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}

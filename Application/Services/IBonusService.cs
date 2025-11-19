using SparkUpSolution.Application.DTOs;
using SparkUpSolution.Application.Requests;

namespace SparkUpSolution.Application.Services
{
    public interface IBonusService
    {
        Task<PagedResult<BonusDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<BonusDTO> CreateAsync(CreateBonusRequest request, CancellationToken cancellationToken);
        Task<BonusDTO> UpdateAsync(Guid id, UpdateBonusRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken); // could be a hard delete or “deactivate”
    }
}

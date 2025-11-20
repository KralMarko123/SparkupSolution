using SparkUpSolution.Application.DTOs;
using SparkUpSolution.Application.Requests;

namespace SparkUpSolution.Application.Services
{
    public interface IBonusService
    {
        Task<PagedResult<BonusDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<BonusDTO> CreateAsync(CreateBonusRequest request, CancellationToken cancellationToken = default);
        Task<BonusDTO> UpdateAsync(Guid id, UpdateBonusRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default); // could be a hard delete or “deactivate”
    }
}

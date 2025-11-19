using AutoMapper;
using SparkUpSolution.Application.DTOs;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Infrastructure.Repositories;

namespace SparkUpSolution.Application.Services
{
    public class BonusService : IBonusService
    {
        private readonly IMapper mapper;
        private readonly IBonusRepository bonusRepository;

        public BonusService(IMapper mapper, IBonusRepository bonusRepository)
        {
            this.mapper = mapper;
            this.bonusRepository = bonusRepository;
        }

        public async Task<PagedResult<BonusDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var pagedBonuses = await bonusRepository.GetAllAsync(pageNumber, pageSize, cancellationToken);
            var dtoItems = mapper.Map<IReadOnlyCollection<BonusDTO>>(pagedBonuses.Items);

            return new PagedResult<BonusDTO>(dtoItems, pagedBonuses.PageNumber, pagedBonuses.PageSize, pagedBonuses.TotalCount);
        }

        Task<BonusDTO> IBonusService.CreateAsync(CreateBonusRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IBonusService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<BonusDTO> IBonusService.UpdateAsync(Guid id, UpdateBonusRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

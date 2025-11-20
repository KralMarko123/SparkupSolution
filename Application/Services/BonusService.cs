using AutoMapper;
using SparkUpSolution.Application.DTOs;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;
using SparkUpSolution.Infrastructure.Logging;
using SparkUpSolution.Infrastructure.Repositories;
using SparkUpSolution.Middlewares;

namespace SparkUpSolution.Application.Services
{
    public class BonusService : IBonusService
    {
        private readonly IMapper mapper;
        private readonly IBonusRepository bonusRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly IBonusAuditRepository auditRepository;
        private readonly ILogger<BonusService> logger;
        private readonly ICurrentOperatorAccessor currentOperator;

        public BonusService(IMapper mapper, IBonusRepository bonusRepository, IPlayerRepository playerRepository, IBonusAuditRepository auditRepository, ILogger<BonusService> logger, ICurrentOperatorAccessor currentOperator)
        {
            this.mapper = mapper;
            this.bonusRepository = bonusRepository;
            this.playerRepository = playerRepository;
            this.auditRepository = auditRepository;
            this.logger = logger;
            this.currentOperator = currentOperator;
        }

        public async Task<PagedResult<BonusDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var pagedBonuses = await bonusRepository.GetAllAsync(pageNumber, pageSize, cancellationToken);
            var dtoItems = mapper.Map<IReadOnlyCollection<BonusDTO>>(pagedBonuses.Items);

            return new PagedResult<BonusDTO>(dtoItems, pagedBonuses.PageNumber, pagedBonuses.PageSize, pagedBonuses.TotalCount);
        }

        public async Task<BonusDTO> CreateAsync(CreateBonusRequest request, CancellationToken cancellationToken)
        {
            var player = await playerRepository.GetByIdAsync(request.PlayerId, cancellationToken) ?? throw new KeyNotFoundException($"Player with Id '{request.PlayerId}' was not found.");

            // Check if there is an active bonus of the same type for the player
            var hasActive = await bonusRepository.HasActiveBonusOfTypeAsync(player.Id, request.Type, null, cancellationToken);

            if (hasActive)
            {
                throw new InvalidOperationException($"Player with Id '{player.Id}' already has an active bonus of type '{request.Type}'.");
            }

            var bonus = mapper.Map<Bonus>(request);

            await bonusRepository.AddAsync(bonus, cancellationToken);
            await bonusRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Bonus of type '{BonusType}' created for player with Id '{PlayerId}'", bonus.Type, bonus.PlayerId);

            return mapper.Map<BonusDTO>(bonus);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var bonus = await bonusRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"Bonus with Id '{id}' was not found.");

            // Mark the bonus as deactivated rather than hard deleting it
            bonus.Status = BonusStatus.Deactivated;

            await bonusRepository.UpdateAsync(bonus, cancellationToken);
            await bonusRepository.SaveChangesAsync(cancellationToken);

            // Log the soft deletion
            var log = new BonusAuditLog
            {
                BonusId = bonus.Id,
                Action = "Delete",
                PerformedAt = DateTime.UtcNow,
                PerformedById = currentOperator.Id,
                PerformedByName = currentOperator.Name,
            };

            await auditRepository.AddAsync(log, cancellationToken);
            await auditRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Bonus with Id '{BonusId}' deactivated by operator with Id '{OperatorId}' and Name '{OperatorName}' at {AuditTimestamp}", bonus.Id, currentOperator.Id, currentOperator.Name, log.PerformedAt.ToString("G"));
        }

        public async Task<BonusDTO> UpdateAsync(Guid id, UpdateBonusRequest request, CancellationToken cancellationToken)
        {
            var bonus = await bonusRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"Bonus with Id '{id}' was not found.");

            // Check if there is an active bonus of the same type for the player if the status is being set to Active
            if (request.Status == BonusStatus.Active)
            {
                var hasActive = await bonusRepository.HasActiveBonusOfTypeAsync(bonus.PlayerId, bonus.Type, bonus.Id, cancellationToken);

                if (hasActive)
                {
                    throw new InvalidOperationException($"Player with Id: '{bonus.PlayerId}' already has an active bonus of type '{bonus.Type}'.");
                }
            }

            // Update only the allowed fields
            bonus.Status = request.Status;

            if (request.Amount.HasValue)
            {
                bonus.Amount = request.Amount.Value;
            }

            await bonusRepository.UpdateAsync(bonus, cancellationToken);
            await bonusRepository.SaveChangesAsync(cancellationToken);

            // Log the update
            var log = new BonusAuditLog
            {
                BonusId = bonus.Id,
                Action = "Update",
                PerformedAt = DateTime.UtcNow,
                PerformedById = currentOperator.Id,
                PerformedByName = currentOperator.Name
            };

            await auditRepository.AddAsync(log, cancellationToken);
            await auditRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Bonus with Id '{BonusId}' updated by operator with Id '{OperatorId}' and Name '{OperatorName}' at {AuditTimestamp}", bonus.Id, currentOperator.Id, currentOperator.Name, log.PerformedAt.ToString("G"));

            return mapper.Map<BonusDTO>(bonus);
        }
    }
}

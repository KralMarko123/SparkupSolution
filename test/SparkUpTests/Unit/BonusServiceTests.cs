using Xunit;
using Moq;
using AutoMapper;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Application.Services;
using SparkUpSolution.Domain.Enums;
using SparkUpSolution.Infrastructure.Logging;
using SparkUpSolution.Infrastructure.Repositories;
using SparkUpSolution.Middlewares;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Application.DTOs;

namespace SparkUpSolution.Tests.Unit
{
    public class BonusServiceTests
    {
        private readonly Mock<IMapper> mapperMock = new();
        private readonly Mock<IBonusRepository> bonusRepositoryMock = new();
        private readonly Mock<IBonusAuditRepository> auditRepositoryMock = new();
        private readonly Mock<IPlayerRepository> playerRepositoryMock = new();
        private readonly Mock<ILogger<BonusService>> loggerMock = new();
        private readonly Mock<ICurrentOperatorAccessor> currentOperatorAccessorMock = new();

        private readonly BonusService bonusService;

        public BonusServiceTests()
        {
            bonusService = new BonusService(mapperMock.Object, bonusRepositoryMock.Object, playerRepositoryMock.Object,
                auditRepositoryMock.Object, loggerMock.Object, currentOperatorAccessorMock.Object);
        }

        [Fact]
        public async Task create_async_throws_key_not_found_if_player_does_not_exist()
        {
            // Arrange
            var request = new CreateBonusRequest
            {
                PlayerId = Guid.NewGuid(),
                Type = BonusType.Welcome,
                Amount = 200
            };

            // Act
            var result = async () => await bonusService.CreateAsync(request);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Player with Id '{request.PlayerId}' was not found.");
        }

        [Fact]
        public async Task create_async_throws_invalid_operation_if_active_bonus_of_same_type_already_exists()
        {
            // Arrange
            var request = new CreateBonusRequest
            {
                PlayerId = Guid.NewGuid(),
                Type = BonusType.Welcome,
                Amount = 200
            };

            playerRepositoryMock.Setup(r => r.GetByIdAsync(request.PlayerId, It.IsAny<CancellationToken>())) .ReturnsAsync(new Player { Id = request.PlayerId });
            bonusRepositoryMock.Setup(r => r.HasActiveBonusOfTypeAsync(request.PlayerId, request.Type, null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = async () => await bonusService.CreateAsync(request);

            // Assert
            await result.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Player with Id '{request.PlayerId}' already has an active bonus of type '{request.Type}'.");
        }

        [Fact]
        public async Task create_async_successfully_returns_created_bonus()
        {
            // Arrange
            var request = new CreateBonusRequest
            {
                PlayerId = Guid.NewGuid(),
                Type = BonusType.Welcome,
                Amount = 200
            };

            var bonusToCreate = new Bonus
            {
                Id = Guid.NewGuid(),
                PlayerId = request.PlayerId,
                Type = request.Type,
                Amount = request.Amount,
                Status = BonusStatus.Pending
            };

            mapperMock.Setup(m => m.Map<Bonus>(request)).Returns(bonusToCreate);
            playerRepositoryMock.Setup(r => r.GetByIdAsync(bonusToCreate.PlayerId, It.IsAny<CancellationToken>())).ReturnsAsync(new Player { Id = bonusToCreate.PlayerId });
            bonusRepositoryMock.Setup(r => r.HasActiveBonusOfTypeAsync(bonusToCreate.PlayerId, bonusToCreate.Type, null, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            bonusRepositoryMock.Setup(r => r.AddAsync(bonusToCreate, It.IsAny<CancellationToken>())).ReturnsAsync(bonusToCreate);
            mapperMock.Setup(m => m.Map<BonusDTO>(bonusToCreate)).Returns(new BonusDTO
            {
                Id = bonusToCreate.Id,
                PlayerId = bonusToCreate.PlayerId,
                Type = bonusToCreate.Type.ToString(),
                Amount = bonusToCreate.Amount,
                Status = bonusToCreate.Status.ToString()
            });

            // Act
            var result = await bonusService.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(bonusToCreate.Id);
            result.PlayerId.Should().Be(bonusToCreate.PlayerId);
            result.Type.Should().Be(bonusToCreate.Type.ToString());
            result.Amount.Should().Be(bonusToCreate.Amount);
            result.Status.Should().Be(bonusToCreate.Status.ToString());
        }

        [Fact]
        public async Task delete_async_throws_key_not_found_if_bonus_does_not_exist()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var result = async () => await bonusService.DeleteAsync(randomId);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Bonus with Id '{randomId}' was not found.");
        }

        [Fact]
        public async Task delete_async_successfully_deactivates_a_bonus()
        {
            // Arrange
            var bonusToDeactivate = new Bonus
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                Type = BonusType.FreeSpins,
                Amount = 100,
                Status = BonusStatus.Active
            };

            bonusRepositoryMock.Setup(r => r.GetByIdAsync(bonusToDeactivate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(bonusToDeactivate);

            // Act
            await bonusService.DeleteAsync(bonusToDeactivate.Id);

            // Assert
            bonusToDeactivate.Status.Should().Be(BonusStatus.Deactivated);
        }

        [Fact]
        public async Task update_async_throws_key_not_found_if_bonus_does_not_exist()
        {
            // Arrange
            var request = new UpdateBonusRequest
            {
                Status = BonusStatus.Active,
                Amount = 200
            };
            var randomId = Guid.NewGuid();

            // Act
            var result = async () => await bonusService.UpdateAsync(randomId, request);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Bonus with Id '{randomId}' was not found.");
        }

        [Fact]
        public async Task update_async_throws_invalid_operation_if_active_bonus_of_same_type_already_exists()
        {
            // Arrange
            var bonusToUpdate = new Bonus
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                Type = BonusType.FreeSpins,
                Amount = 100,
                Status = BonusStatus.Pending
            };

            var request = new UpdateBonusRequest
            {
                Status = BonusStatus.Active,
                Amount = 200
            };

            bonusRepositoryMock.Setup(r => r.GetByIdAsync(bonusToUpdate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(bonusToUpdate);
            bonusRepositoryMock.Setup(r => r.HasActiveBonusOfTypeAsync(bonusToUpdate.PlayerId, bonusToUpdate.Type, bonusToUpdate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = async () => await bonusService.UpdateAsync(bonusToUpdate.Id, request);

            // Assert
            await result.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Player with Id '{bonusToUpdate.PlayerId}' already has an active bonus of type '{bonusToUpdate.Type}'.");
        }

        [Fact]
        public async Task update_async_successfully_returns_updated_bonus()
        {
            // Arrange
            var bonusToUpdate = new Bonus
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                Type = BonusType.FreeSpins,
                Amount = 100,
                Status = BonusStatus.Pending
            };

            var request = new UpdateBonusRequest
            {
                Status = BonusStatus.Active,
                Amount = 200
            };

            bonusRepositoryMock.Setup(r => r.GetByIdAsync(bonusToUpdate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(bonusToUpdate);
            bonusRepositoryMock.Setup(r => r.HasActiveBonusOfTypeAsync(bonusToUpdate.PlayerId, bonusToUpdate.Type, bonusToUpdate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            mapperMock.Setup(m => m.Map<BonusDTO>(bonusToUpdate)).Returns(new BonusDTO
            {
                Id = bonusToUpdate.Id,
                PlayerId = bonusToUpdate.PlayerId,
                Type = bonusToUpdate.Type.ToString(),
                Amount = request.Amount.Value,
                Status = request.Status.ToString()
            });

            // Act
            var result = await bonusService.UpdateAsync(bonusToUpdate.Id, request);

            // Assert
            bonusToUpdate.Status.Should().Be(request.Status);
            bonusToUpdate.Amount.Should().Be(request.Amount);

            result.Should().NotBeNull();
            result.Id.Should().Be(bonusToUpdate.Id);
            result.PlayerId.Should().Be(bonusToUpdate.PlayerId);
            result.Type.Should().Be(bonusToUpdate.Type.ToString());
            result.Amount.Should().Be(bonusToUpdate.Amount);
            result.Status.Should().Be(bonusToUpdate.Status.ToString());
        }

        [Fact]
        public async Task get_all_async_returns_all_bonuses()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var bonuses = new List<Bonus>
            {
                new Bonus
                {
                    Id = Guid.NewGuid(),
                    PlayerId = Guid.NewGuid(),
                    Type = BonusType.Welcome,
                    Amount = 100,
                    Status = BonusStatus.Active
                },
                new Bonus
                {
                    Id = Guid.NewGuid(),
                    PlayerId = Guid.NewGuid(),
                    Type = BonusType.FreeSpins,
                    Amount = 50,
                    Status = BonusStatus.Pending
                }
            };

            var resultingBonuses = new PagedResult<Bonus>(bonuses, pageNumber, pageSize, bonuses.Count);

            bonusRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(resultingBonuses);
            mapperMock.Setup(m => m.Map<IReadOnlyCollection<BonusDTO>>(resultingBonuses.Items)).Returns(resultingBonuses.Items.Select(b => new BonusDTO
            {
                Id = b.Id,
                PlayerId = b.PlayerId,
                Type = b.Type.ToString(),
                Amount = b.Amount,
                Status = b.Status.ToString()
            }).ToList());

            // Act
            var result = await bonusService.GetAllAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(bonuses.Count);
            result.Items.Should().BeEquivalentTo(bonuses.Select(b => new BonusDTO
            {
                Id = b.Id,
                PlayerId = b.PlayerId,
                Type = b.Type.ToString(),
                Amount = b.Amount,
                Status = b.Status.ToString()
            }));
        }
    }
}

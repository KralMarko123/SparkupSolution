using Xunit;
using Moq;
using AutoMapper;
using SparkUpSolution.Application.Mapping;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Application.Services;
using SparkUpSolution.Domain.Enums;
using SparkUpSolution.Infrastructure.Logging;
using SparkUpSolution.Infrastructure.Repositories;
using SparkUpSolution.Middlewares;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SparkUpSolution.Domain.Entities;

namespace SparkUpSolution.Tests.Unit
{
    public class BonusServiceTests
    {
        private readonly IMapper mapper;
        private readonly Mock<IBonusRepository> bonusRepositoryMock = new();
        private readonly Mock<IBonusAuditRepository> auditRepositoryMock = new();
        private readonly Mock<IPlayerRepository> playerRepositoryMock = new();
        private readonly Mock<ILogger<BonusService>> loggerMock = new();
        private readonly Mock<ICurrentOperatorAccessor> currentOperatorAccessorMock = new();

        private readonly BonusService bonusService;

        public BonusServiceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<BonusProfile>());
            mapper = mapperConfig.CreateMapper();

            bonusService = new BonusService(mapper, bonusRepositoryMock.Object, playerRepositoryMock.Object,
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
            var result = async () => await bonusService.CreateAsync(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Player with Id '{request.PlayerId}' was not found.");
        }

        [Fact]
        public async Task create_async_throws_invalid_operation_if_active_bonus_already_exists()
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
            var result = async () => await bonusService.CreateAsync(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Player with Id '{request.PlayerId}' already has an active bonus of type '{request.Type}'.");
        }
    }
}

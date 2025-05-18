using AtelierCleanApp.Application.Contracts;
using AtelierCleanApp.Application.Services;
using AtelierCleanApp.Infrastructure.Contracts;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Domain.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using AutoMapper;

namespace AtelierCleanApp.Application.Tests.Services;


public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<ILogger<IPlayerService>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PlayerService _playerService;

    public PlayerServiceTests()
    {
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _mockLogger = new Mock<ILogger<IPlayerService>>();
        _mockMapper = new Mock<IMapper>();
        _playerService = new PlayerService(_mockPlayerRepository.Object, _mockLogger.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WhenPlayerExists_ShouldReturnMappedPlayerDto()
    {
        // Arrange
        var playerId = 1;
        var playerEntity = new Player
        {
            Id = playerId, FirstName = "Test", LastName = "Player", ShortName = "T.PLA", Sex = "M",
            Picture = "test.png", CountryCode = "TST",
            Country = new Country { Code = "TST", Picture = "tst_country.png" },
            Data = new PlayerData { Rank = 5, Points = 1000, Age = 25, Weight = 70000, Height = 180, LastResults = new List<string> { "1", "0", "1" } }
        };
        var playerDto = new PlayerDto
        {
            Id = playerEntity.Id, FirstName = playerEntity.FirstName, LastName = playerEntity.LastName, Picture = playerEntity.Picture,
            Country = new CountryDto { Code = playerEntity.CountryCode, Picture = playerEntity.Country.Picture },
            Data = new PlayerDataDto { Rank = playerEntity.Data.Rank, Points = playerEntity.Data.Points, LastResults = playerEntity.Data.LastResults }
        };
        _mockPlayerRepository.Setup(repo => repo.GetPlayerByIdAsync(playerId)).ReturnsAsync(playerEntity);
        _mockMapper.Setup(m => m.Map<PlayerDto>(playerEntity)).Returns(playerDto);

        // Act
        var resultDto = await _playerService.GetPlayerByIdAsync(playerId);

        // Assert
        resultDto.Should().NotBeNull();
        resultDto.Id.Should().Be(playerEntity.Id);
        resultDto.FirstName.Should().Be(playerEntity.FirstName);
        resultDto.Picture.Should().Be(playerEntity.Picture);
        resultDto.Country.Code.Should().Be(playerEntity.CountryCode);
        resultDto.Country.Picture.Should().Be(playerEntity.Country.Picture);
        resultDto.Data.Rank.Should().Be(playerEntity.Data.Rank);
        resultDto.Data.LastResults.Should().BeEquivalentTo(playerEntity.Data.LastResults);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WhenPlayerDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var playerId = 99;
        _mockPlayerRepository.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                .ReturnsAsync((Player?)null);

        // Act
        var result = await _playerService.GetPlayerByIdAsync(playerId);

        // Assert
        result.Should().BeNull();
        _mockPlayerRepository.Verify(repo => repo.GetPlayerByIdAsync(playerId), Times.Once);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithZeroId_ShouldReturnNull_AndLogWarning()
    {
        // Arrange
        var playerId = 0;

        // Act
        var result = await _playerService.GetPlayerByIdAsync(playerId);

        // Assert
        result.Should().BeNull();
        _mockPlayerRepository.Verify(repo => repo.GetPlayerByIdAsync(It.IsAny<int>()), Times.Never); // Ensure repo not called

        // Verify logging (optional, but good for checking behavior)
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Attempted to get player with invalid ID: {playerId}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithNegativeId_ShouldReturnNull_AndLogWarning()
    {
        // Arrange
        var playerId = -1;

        // Act
        var result = await _playerService.GetPlayerByIdAsync(playerId);

        // Assert
        result.Should().BeNull();
        _mockPlayerRepository.Verify(repo => repo.GetPlayerByIdAsync(It.IsAny<int>()), Times.Never);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Attempted to get player with invalid ID: {playerId}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WhenRepositoryThrowsException_ShouldLogAndThrow()
    {
        // Arrange
        var playerId = 1;
        var repositoryException = new InvalidOperationException("Database connection error");
        _mockPlayerRepository.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                .ThrowsAsync(repositoryException);

        // Act
        Func<Task> act = async () => await _playerService.GetPlayerByIdAsync(playerId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection error"); // Ensure the original exception is re-thrown

        _mockPlayerRepository.Verify(repo => repo.GetPlayerByIdAsync(playerId), Times.Once);

        // Verify logging of the error
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"An error occurred while retrieving player with ID: {playerId} from repository.")),
                repositoryException, // Check that the correct exception was logged
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
        public async Task GetAllPlayersSortedByRankAsync_WhenPlayersExist_ShouldReturnMappedPlayerDtosSorted()
        {
            // Arrange
            var playerEntities = new List<Player>
            {
                new Player { Id = 2, FirstName = "A", CountryCode = "C1", Country = new Country{Code = "C1"}, Data = new PlayerData { Rank = 1 } },
                new Player { Id = 1, FirstName = "B", CountryCode = "C2", Country = new Country{Code = "C2"}, Data = new PlayerData { Rank = 2 } }
            }.AsReadOnly();
            var playerDtos = new List<PlayerDto>
            {
                new PlayerDto { Id = 2, FirstName = "A", Country = new CountryDto{Code = "C1"}, Data = new PlayerDataDto { Rank = 1 } },
                new PlayerDto { Id = 1, FirstName = "B", Country = new CountryDto{Code = "C2"}, Data = new PlayerDataDto { Rank = 2 } }
            }.AsReadOnly();
            _mockPlayerRepository.Setup(repo => repo.GetAllPlayersSortedByRankAsync()).ReturnsAsync(playerEntities);
            _mockMapper.Setup(m => m.Map<IReadOnlyList<PlayerDto>>(playerEntities)).Returns(playerDtos);

            // Act
            var resultDtos = await _playerService.GetAllPlayersSortedByRankAsync();

            // Assert
            resultDtos.Should().NotBeNull().And.HaveCount(2);
            resultDtos.Should().BeInAscendingOrder(p => p.Data.Rank);
            resultDtos.First().Id.Should().Be(playerEntities.First().Id);
        }
}

using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Infrastructure.Persistence;
using AtelierCleanApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace AtelierCleanApp.Infrastructure.Tests.Repositories;

public class PlayerRepositoryTests
{
    private readonly DbContextOptions<AtelierCleanAppDbContext> _dbContextOptions;
    private readonly Mock<ILogger<PlayerRepository>> _mockLogger;

    public PlayerRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AtelierCleanAppDbContext>()
            .UseInMemoryDatabase(databaseName: $"PlayerRepositoryTests_{Guid.NewGuid()}")
            .Options;
        _mockLogger = new Mock<ILogger<PlayerRepository>>();
    }
    private AtelierCleanAppDbContext CreateFreshContext() => new AtelierCleanAppDbContext(_dbContextOptions);

    [Fact]
    public async Task GetPlayerByIdAsync_WhenPlayerExists_ShouldReturnCorrectPlayerWithIncludedEntities()
    {
        // Arrange
        await using var context = CreateFreshContext();
        var country = new Country { Code = "ESP", Picture = "esp.png" };
        var expectedPlayer = new Player
        {
            Id = 1, FirstName = "Rafael", LastName = "Nadal", ShortName = "R.NAD", Sex = "M", Picture = "nadal.png",
            CountryCode = "ESP", Country = country,
            Data = new PlayerData { Rank = 1, Points = 8000, Weight = 85000, Height = 185, Age = 33, LastResults = new List<string> { "1", "0", "1" } }
        };
        // Add Country first if it's a separate entity and not cascaded
        // context.Countries.Add(country); // Not needed if Player adds/updates Country via FK
        context.Players.Add(expectedPlayer);
        await context.SaveChangesAsync();

        var playerRepository = new PlayerRepository(context, _mockLogger.Object);

        // Act
        var actualPlayer = await playerRepository.GetPlayerByIdAsync(expectedPlayer.Id);

        // Assert
        actualPlayer.Should().NotBeNull();
        actualPlayer.Id.Should().Be(expectedPlayer.Id);
        actualPlayer.FirstName.Should().Be(expectedPlayer.FirstName);
        actualPlayer.Country.Should().NotBeNull();
        actualPlayer.Country.Code.Should().Be(expectedPlayer.Country.Code);
        actualPlayer.Data.Should().NotBeNull();
        actualPlayer.Data.Rank.Should().Be(expectedPlayer.Data.Rank);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WhenPlayerDoesNotExist_ShouldReturnNull()
    {
        await using var context = CreateFreshContext();
        var playerRepository = new PlayerRepository(context, _mockLogger.Object);
        var result = await playerRepository.GetPlayerByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithZeroId_ShouldReturnNull()
    {
        await using var context = CreateFreshContext();
        var playerRepository = new PlayerRepository(context, _mockLogger.Object);
        var result = await playerRepository.GetPlayerByIdAsync(0);
        result.Should().BeNull();
    }
    [Fact]
    public async Task GetPlayerByIdAsync_WithNegativeId_ShouldReturnNull()
    {
        await using var context = CreateFreshContext();
        var playerRepository = new PlayerRepository(context, _mockLogger.Object);
        var result = await playerRepository.GetPlayerByIdAsync(-5);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllPlayersSortedByRankAsync_WhenPlayersExist_ShouldReturnSortedPlayersByDataRankIncludingData()
    {
        // Arrange
        await using var context = CreateFreshContext();
        var countrySrb = new Country { Code = "SRB", Picture = "srb.png" };
        var countryEsp = new Country { Code = "ESP", Picture = "esp.png" };
        var countrySui = new Country { Code = "SUI", Picture = "sui.png" };
        context.Countries.AddRange(countrySrb, countryEsp, countrySui); // Add countries first

        var player1 = new Player { Id = 1, FirstName = "Novak", CountryCode = "SRB", Country = countrySrb, Data = new PlayerData { Rank = 2, PlayerId = 1 } };
        var player2 = new Player { Id = 2, FirstName = "Rafael", CountryCode = "ESP", Country = countryEsp, Data = new PlayerData { Rank = 1, PlayerId = 2 } };
        var player3 = new Player { Id = 3, FirstName = "Roger", CountryCode = "SUI", Country = countrySui, Data = new PlayerData { Rank = 3, PlayerId = 3 } };
        context.Players.AddRange(player1, player2, player3);
        await context.SaveChangesAsync();

        var playerRepository = new PlayerRepository(context, _mockLogger.Object);

        // Act
        var result = await playerRepository.GetAllPlayersSortedByRankAsync();

        // Assert
        result.Should().NotBeNullOrEmpty().And.HaveCount(3);
        result.Should().BeInAscendingOrder(p => p.Data.Rank);
        result.First().Id.Should().Be(player2.Id);
        result.All(p => p.Data != null).Should().BeTrue(); // Ensure Data is loaded
        result.All(p => p.Country != null).Should().BeTrue(); // Ensure Country is loaded
    }

    [Fact]
    public async Task GetAllPlayersSortedByRankAsync_WhenNoPlayersExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = CreateFreshContext(); // Empty context
        var playerRepository = new PlayerRepository(context, _mockLogger.Object);

        // Act
        var result = await playerRepository.GetAllPlayersSortedByRankAsync();

        // Assert
        result.Should().NotBeNull(); // Should be an empty list, not null
        result.Should().BeEmpty();
    }
}

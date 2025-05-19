using AtelierCleanApp.Application.Contracts;
using AtelierCleanApp.Application.Services;
using AtelierCleanApp.Infrastructure.Contracts;
using AtelierCleanApp.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace AtelierCleanApp.Application.Tests.Services;

public class PlayerStatisticsServiceTests
{
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<ILogger<IPlayerStatisticsService>> _mockLogger;
    private readonly PlayerStatisticsService _playerStatisticsService;

    public PlayerStatisticsServiceTests()
    {
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _mockLogger = new Mock<ILogger<IPlayerStatisticsService>>();
        _playerStatisticsService = new PlayerStatisticsService(_mockPlayerRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void CalculateAverageBmi_WithValidPlayers_ReturnsCorrectAverage()
    {
        var players = new List<Player>
        {
            new Player { Data = new PlayerData { Height = 180, Weight = 72000 } },
            new Player { Data = new PlayerData { Height = 160, Weight = 64000 } }
        };

        var bmi = InvokePrivate<double>(_playerStatisticsService, "CalculateAverageBmi", players);
        bmi.Should().BeApproximately(23.61, 0.01); // Pre-calculated
    }

    [Fact]
    public void CalculateMedianHeight_WithOddNumberOfPlayers_ReturnsMiddle()
    {
        var players = new List<Player>
        {
            new Player { Data = new PlayerData { Height = 170, Weight = 70000 } }, // BMI ~24.22
            new Player { Data = new PlayerData { Height = 180, Weight = 81000 } }, // BMI ~25.00
            new Player { Data = new PlayerData { Height = 160, Weight = 55000 } }  // BMI ~21.48
        };

        var median = InvokePrivate<double>(_playerStatisticsService, "CalculateMedianHeight", players);
        median.Should().Be(170);
    }

    [Fact]
    public void CalculateMedianHeight_WithEvenNumberOfPlayers_ReturnsAverageOfMiddle()
    {
        var players = new List<Player>
        {
            new Player { Data = new PlayerData { Height = 160 } },
            new Player { Data = new PlayerData { Height = 170 } },
            new Player { Data = new PlayerData { Height = 180 } },
            new Player { Data = new PlayerData { Height = 190 } }
        };

        var median = InvokePrivate<double>(_playerStatisticsService, "CalculateMedianHeight", players);
        median.Should().Be(175);
    }

    [Fact]
    public void CalculateCountryWithBestWinRatio_ReturnsCorrectCountry()
    {
        var players = new List<Player>
        {
            new Player { Country = new Country { Code = "USA" }, Data = new PlayerData { LastResults = new List<string> { "1", "1", "0" } } },
            new Player { Country = new Country { Code = "FRA" }, Data = new PlayerData { LastResults = new List<string> { "1", "0" } } },
            new Player { Country = new Country { Code = "USA" }, Data = new PlayerData { LastResults = new List<string> { "1", "1" } } },
        };

        var bestCountry = InvokePrivate<string>(_playerStatisticsService, "CalculateCountryWithBestWinRatio", players);
        bestCountry.Should().Be("USA");
    }

    [Fact]
    public void CalculateCountryWithBestWinRatio_WithNoResults_ReturnsNull()
    {
        var players = new List<Player>
        {
            new Player { Country = new Country { Code = "ESP" }, Data = new PlayerData { LastResults = new List<string>() } },
        };

        var bestCountry = InvokePrivate<string>(_playerStatisticsService, "CalculateCountryWithBestWinRatio", players);
        bestCountry.Should().BeEmpty();
    }

    [Fact]
        public async Task GetStatisticsAsync_ShouldHandleMixedValidAndInvalidData()
        {
            var players = new List<Player>
            {
                new Player { Data = new PlayerData { Height = 170, Weight = 70000 }, Country = new Country { Code = "FRA" } },
                new Player { Data = new(), Country = new() },
                new Player { Data = new PlayerData { Height = 0, Weight = 0 }, Country = new Country { Code = "FRA" } }
            };

            _mockPlayerRepository.Setup(r => r.GetAllPlayersAsync()).ReturnsAsync(players);

            var result = await _playerStatisticsService.GetPlayersStatisticsAsync();

            result.MedianHeight.Should().Be(170);
            result.AverageBmi.Should().BeApproximately(24.22, 0.01);
            result.CountryWithHighestWinRatio.Should().BeEmpty();
        }

    private T? InvokePrivate<T>(object obj, string methodName, params object[] parameters)
    {
        var method = obj.GetType()
                        .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (method == null)
            throw new MissingMethodException($"Method '{methodName}' not found on type '{obj.GetType().FullName}'.");
        var result = method.Invoke(obj, parameters);
        if (result is null && typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            throw new InvalidOperationException($"Method '{methodName}' returned null for non-nullable type '{typeof(T).FullName}'.");
        return (T?)result;
    }
}
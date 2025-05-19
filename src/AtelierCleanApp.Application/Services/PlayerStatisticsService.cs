using AtelierCleanApp.Application.Contracts;
using AtelierCleanApp.Infrastructure.Contracts;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Domain.Dtos;
using AtelierCleanApp.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace AtelierCleanApp.Application.Services;

public class PlayerStatisticsService : IPlayerStatisticsService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<IPlayerStatisticsService> _logger;

    public PlayerStatisticsService(IPlayerRepository playerRepository, ILogger<IPlayerStatisticsService> logger)
    {
        _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<PlayerStatisticsDto> GetPlayersStatisticsAsync()
    {
        try
        {
            var players = await _playerRepository.GetAllPlayersAsync();

            if (players == null || players.Count == 0)
                return new PlayerStatisticsDto();

            var bmi = CalculateAverageBmi(players);
            var medianHeight = CalculateMedianHeight(players);
            var bestCountry = CalculateCountryWithBestWinRatio(players);

            return new PlayerStatisticsDto
            {
                CountryWithHighestWinRatio = bestCountry,
                AverageBmi = bmi,
                MedianHeight = medianHeight
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.StatisticsComputeError);
            throw new ApplicationException(Messages.ErrorMessages.StatisticsApplicationException, ex);
        }
    }

    /// <summary>
    /// Calculates the average BMI of players.
    /// </summary>
    /// <param name="players">List of players.</param>
    /// <returns>Average BMI rounded to two decimal places.</returns>
    private double CalculateAverageBmi(IReadOnlyList<Player> players)
    {
        double bmiSum = 0;
        int count = 0;

        foreach (var player in players)
        {
            var data = player.Data;
            if (data.Height > 0 && data.Weight > 0)
            {
                var heightInMeters = data.Height / 100.0;
                var weightInKg = data.Weight / 1000.0;
                bmiSum += weightInKg / (heightInMeters * heightInMeters);
                count++;
            }
        }

        return count > 0 ? Math.Round(bmiSum / count, 2) : 0;
    }

    /// <summary>
    /// Calculates the median height of players.
    /// </summary>
    /// <param name="players">List of players.</param>
    /// <returns>Median height in centimeters.</returns>
    private double CalculateMedianHeight(IReadOnlyList<Player> players)
    {
        var heights = players
            .Where(p => p.Data?.Height > 0)
            .Select(p => p.Data.Height)
            .OrderBy(h => h)
            .ToList();

        if (heights.Count == 0)
            return 0;

        int mid = heights.Count / 2;
        return heights.Count % 2 == 0
            ? (heights[mid - 1] + heights[mid]) / 2.0
            : heights[mid];
    }


    /// <summary>
    /// Calculates the country with the best win ratio.
    /// </summary>
    /// <param name="players">List of players.</param>
    /// <returns>Country code with the best win ratio.</returns>
    private string CalculateCountryWithBestWinRatio(IReadOnlyList<Player> players)
    {
        var countryStats = new Dictionary<string, (int Wins, int Total)>();

        foreach (var player in players)
        {
            var data = player.Data;
            if (data?.LastResults == null || data.LastResults.Count == 0 || player.Country == null)
                continue;

            int wins = data.LastResults.Count(r => r == "1");
            int total = data.LastResults.Count;

            var code = player.Country.Code;
            if (countryStats.TryGetValue(code, out var current))
            {
                countryStats[code] = (current.Wins + wins, current.Total + total);
            }
            else
            {
                countryStats[code] = (wins, total);
            }
        }

        string bestCountry = string.Empty;
        double bestRatio = -1;

        foreach (var (code, (wins, total)) in countryStats)
        {
            if (total == 0) continue;
            var ratio = (double)wins / total;
            if (ratio > bestRatio)
            {
                bestRatio = ratio;
                bestCountry = code;
            }
        }

        return bestCountry;
    }
}
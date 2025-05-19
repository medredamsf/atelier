using AtelierCleanApp.Domain.Dtos;

namespace AtelierCleanApp.Application.Contracts;

/// <summary>
/// Represents a service for retrieving player statistics.
/// This service provides methods to get player statistics data.
/// </summary>
public interface IPlayerStatisticsService
{
    /// <summary>
    /// Asynchronously retrieves player statistics data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the player statistics data.</returns>
    Task<PlayerStatisticsDto> GetPlayersStatisticsAsync();
}
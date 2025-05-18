using AtelierCleanApp.Domain.Dtos;

namespace AtelierCleanApp.Application.Contracts;

/// <summary>
/// Interface for player service.
/// This interface defines the contract for player-related operations.
/// It provides methods to retrieve player information.
/// </summary>
public interface IPlayerService
{
    /// <summary>
    /// Gets a player by their ID.
    /// </summary>
    /// <param name="playerId">l'identifiant du joueur</param>
    /// <returns>Les informations du joueur dont l'identifiant a été saisi</returns>
    Task<PlayerDto?> GetPlayerByIdAsync(int playerId);

    /// <summary>
    /// Gets all players sorted by rank.
    /// </summary>
    /// <returns>Read only list ordered by rank</returns>
    Task<IReadOnlyList<PlayerDto>> GetAllPlayersSortedByRankAsync();
}

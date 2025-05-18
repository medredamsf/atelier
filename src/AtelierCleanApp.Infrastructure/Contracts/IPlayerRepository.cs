using AtelierCleanApp.Domain.Entities;

namespace AtelierCleanApp.Infrastructure.Contracts;

/// <summary>
/// Interface for player repository.
/// This interface defines the contract for player data access operations.
/// </summary>
public interface IPlayerRepository
{
    /// <summary>
    /// Gets a player by their ID.
    /// This method retrieves a player from the data source using their unique identifier.
    /// If the player is not found, it returns null.
    /// </summary>
    /// <param name="playerId">l'identifiant du joueur</param>
    /// <returns>Les informations du joueur dont l'identifiant a été saisi</returns>
    Task<Player?> GetPlayerByIdAsync(int playerId);

    /// <summary>
    /// Gets all players sorted by rank.
    /// This method retrieves all players from the data source and sorts them by their rank.
    /// The rank is assumed to be an integer where a lower number indicates a better rank.
    /// For example, a player with rank 1 is better than a player with rank 2.
    /// Players are sorted in ascending order of rank.
    /// </summary>
    /// <returns>Une liste en lecture seule des joueurs triés.</returns>
    Task<IReadOnlyList<Player>> GetAllPlayersSortedByRankAsync();
}

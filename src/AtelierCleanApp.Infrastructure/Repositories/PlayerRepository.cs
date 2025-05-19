
using AtelierCleanApp.Infrastructure.Contracts;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AtelierCleanApp.Domain.Constants;

namespace AtelierCleanApp.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly AtelierCleanAppDbContext _dbContext;
    private readonly ILogger<IPlayerRepository> _logger;

    public PlayerRepository(AtelierCleanAppDbContext dbContext, ILogger<IPlayerRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<Player?> GetPlayerByIdAsync(int playerId)
    {
        if (playerId <= 0)
        {
            _logger.LogWarning(Messages.WarningMessages.InvalidPlayerIDRepository, playerId);
            return null;
        }
        try
        {
            return await _dbContext.Players
            .Include(p => p.Country)
            .Include(p => p.Data)
            .FirstOrDefaultAsync(p => p.Id == playerId);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, Messages.ErrorMessages.UnexpectedDatabaseErrorPlayerID, playerId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.UnexpectedErrorOccurredPlayerID, playerId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Player>> GetAllPlayersSortedByRankAsync()
    {
        try
        {
            return await _dbContext.Players
            .Include(p => p.Country)
            .Include(p => p.Data)
            .OrderBy(p => p.Data.Rank)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.UnexpectedErrorOccurredRank);
            throw; // Re-throw to be handled by the service layer or an exception middleware
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Player>> GetAllPlayersAsync()
    {
        try
        {
            return await _dbContext.Players
                                    .Include(p => p.Country)
                                    .Include(p => p.Data)
                                    .AsNoTracking() // Good for read-only operations
                                    .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.UnexpectedErrorOccurredGetAllPlayers);
            throw; // Re-throw to be handled by the service layer or an exception middleware
        }
    }
}
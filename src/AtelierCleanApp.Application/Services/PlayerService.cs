using AtelierCleanApp.Application.Contracts;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Domain.Constants;
using AtelierCleanApp.Domain.Dtos;
using AtelierCleanApp.Infrastructure.Contracts;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace AtelierCleanApp.Application.Services;

/// <summary>
/// Service for managing player-related operations.
/// This service interacts with the player repository to perform data access operations.
/// It provides methods to retrieve player information.
/// </summary>
public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<IPlayerService> _logger;
    private readonly IMapper _mapper;

    public PlayerService(IPlayerRepository playerRepository, ILogger<IPlayerService> logger, IMapper mapper)
    {
        _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<PlayerDto?> GetPlayerByIdAsync(int playerId)
    {
        if (playerId <= 0)
        {
            _logger.LogWarning(Messages.WarningMessages.InvalidPlayerIDApplication, playerId);
            return null;
        }
        try
        {
            _logger.LogInformation("Attempting to retrieve player with ID: {PlayerId} from repository.", playerId);
            var player = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (player == null)
            {
                _logger.LogInformation(Messages.InformationMessages.PlayerNotFound, playerId);
            }
            else
            {
                _logger.LogInformation(Messages.InformationMessages.GetPlayerSuccess, playerId);
            }
            return _mapper.Map<PlayerDto>(player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving player with ID: {PlayerId} from repository.", playerId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PlayerDto>> GetAllPlayersSortedByRankAsync()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve all players sorted by rank from repository.");
            var players = await _playerRepository.GetAllPlayersSortedByRankAsync();
            _logger.LogInformation("Successfully retrieved {PlayerCount} players sorted by rank.", players.Count);
            return _mapper.Map<IReadOnlyList<PlayerDto>>(players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all players sorted by rank from repository.");
            throw; // Re-throw to be handled by the controller or an exception middleware
        }
    }
}
using AtelierCleanApp.Application.Contracts;
using AtelierCleanApp.Domain.Constants;
using AtelierCleanApp.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AtelierCleanApp.Api.Controllers;


[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IPlayerStatisticsService _playerStatisticsService;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(IPlayerService playerService, IPlayerStatisticsService playerStatisticsService, ILogger<PlayersController> logger)
    {
        _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        _playerStatisticsService = playerStatisticsService ?? throw new ArgumentNullException(nameof(playerStatisticsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets a specific player by their ID.
    /// </summary>
    /// <param name="id">The ID of the player.</param>
    /// <returns>The player if found; otherwise, a 404 Not Found.</returns>
    /// <response code="200">Returns the requested player.</response>
    /// <response code="404">If the player with the specified ID is not found.</response>
    /// <response code="400">If the ID is invalid (e.g., less than or equal to 0).</response>
    /// <response code="500">If an unexpected error occurred.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPlayerById(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning(Messages.WarningMessages.InvalidPlayerIDController, id);
            return BadRequest(Messages.BadRequestMessages.PlayerIDBadRequest);
        }
        try
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogInformation(Messages.InformationMessages.PlayerNotFoundController, id);
                return NotFound(Messages.NotFoundMessages.PlayerNotFound);
            }

            return Ok(player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.UnhandledExceptionGetPlayerID, id);
            return StatusCode(StatusCodes.Status500InternalServerError, Messages.ErrorMessages.UnexpectedError);
        }
    }

    /// <summary>
    /// Gets all players sorted by their rank.
    /// The players are sorted in ascending order based on their rank.
    /// </summary>
    /// <returns>A list of players sorted by rank.</returns>
    /// <response code="200">Returns the list of players</response>
    /// <response code="404">If no player found.</response>
    /// <response code="500">Unexpected error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlayerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPlayersSortedByRank()
    {
        _logger.LogInformation(Messages.InformationMessages.GetAllPlayersSortedByRankEndpoint);
        try
        {
            var players = await _playerService.GetAllPlayersSortedByRankAsync();

            if (players == null || !players.Any())
            {
                _logger.LogInformation(Messages.InformationMessages.PlayersNotFoundController);
                return NotFound(Messages.NotFoundMessages.PlayerNotFound);
            }

            return Ok(players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.UnhandledExceptionGetAllPlayers);
            return StatusCode(StatusCodes.Status500InternalServerError, Messages.ErrorMessages.UnexpectedError);
        }
    }

    /// <summary>
    /// Gets player statistics.
    /// This includes average BMI, median height, and the country with the highest win ratio.
    /// </summary>
    /// <returns>Players statistics</returns>
    /// <response code="200">Returns the statistics of players</response>
    /// <response code="404">If no player data found.</response>
    /// <response code="500">Unexpected error happens.</response>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(PlayerStatisticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPlayersStatistics()
    {
        try
        {
            var statistics = await _playerStatisticsService.GetPlayersStatisticsAsync();
            if (statistics == null)
            {
                _logger.LogWarning(Messages.WarningMessages.NoPlayerData);
                return NotFound(Messages.NotFoundMessages.PlayersStatisticsNotFound);
            }

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ErrorMessages.UnhandledExceptionGetPlayersStatistics);
            return StatusCode(StatusCodes.Status500InternalServerError, Messages.ErrorMessages.UnexpectedErrorOccurredGetPlayersStatistics);
        }
    }
}

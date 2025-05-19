using AtelierCleanApp.Api.Controllers;
using AtelierCleanApp.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

using AtelierCleanApp.Domain.Dtos;
using AtelierCleanApp.Domain.Constants;

namespace AtelierCleanApp.Api.Tests.Controllers;

public class PlayersControllerTests
{
    private readonly Mock<IPlayerService> _mockPlayerService;
    private readonly Mock<IPlayerStatisticsService> _mockPlayerStatisticsService;
    private readonly Mock<ILogger<PlayersController>> _mockLogger;
    private readonly PlayersController _playersController;

    public PlayersControllerTests()
    {
        _mockPlayerService = new Mock<IPlayerService>();
        _mockPlayerStatisticsService = new Mock<IPlayerStatisticsService>();
        _mockLogger = new Mock<ILogger<PlayersController>>();

        _playersController = new PlayersController(_mockPlayerService.Object, _mockPlayerStatisticsService.Object, _mockLogger.Object);
    }

    // Tests for GetPlayerById()
    [Fact]
    public async Task GetPlayerById_WhenPlayerExists_ShouldReturnOkObjectResultWithPlayerDto()
    {
        // Arrange
        var playerId = 1;
        var expectedDto = new PlayerDto
        {
            Id = playerId,
            FirstName = "ControllerTest",
            LastName = "Player",
            Country = new CountryDto { Code = "TST", Picture = "tst.png" },
            Data = new PlayerDataDto { Rank = 1 }
        };
        _mockPlayerService.Setup(service => service.GetPlayerByIdAsync(playerId)).ReturnsAsync(expectedDto);

        // Act
        var actionResult = await _playersController.GetPlayerById(playerId);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedDto);
    }

    // Tests for GetAllPlayersSortedByRank()
    [Fact]
    public async Task GetAllPlayersSortedByRank_WhenPlayersExist_ShouldReturnOkObjectResultWithPlayerDtos()
    {
        // Arrange
        var expectedDtos = new List<PlayerDto> {
            new PlayerDto { Id = 1, Data = new PlayerDataDto { Rank = 1 }, Country = new CountryDto() },
            new PlayerDto { Id = 2, Data = new PlayerDataDto { Rank = 2 }, Country = new CountryDto() }
        }.AsReadOnly();
        _mockPlayerService.Setup(service => service.GetAllPlayersSortedByRankAsync()).ReturnsAsync(expectedDtos);

        // Act
        var actionResult = await _playersController.GetAllPlayersSortedByRank();

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedDtos);
    }

    [Fact]
    public async Task GetAllPlayersSortedByRank_WhenNoPlayersExist_ShouldReturnNotFoundResult()
    {
        // Arrange
        _mockPlayerService.Setup(service => service.GetAllPlayersSortedByRankAsync())
                            .ReturnsAsync(new List<PlayerDto>().AsReadOnly()); // Service returns empty list

        // Act
        var actionResult = await _playersController.GetAllPlayersSortedByRank();

        // Assert
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = actionResult as NotFoundObjectResult;
        notFoundResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult?.Value.Should().Be(Messages.NotFoundMessages.PlayerNotFound);
        _mockPlayerService.Verify(service => service.GetAllPlayersSortedByRankAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllPlayersSortedByRank_WhenServiceReturnsNull_ShouldReturnNotFoundResult()
    {
        // Arrange
        _mockPlayerService.Setup(service => service.GetAllPlayersSortedByRankAsync())
                            .ReturnsAsync((IReadOnlyList<PlayerDto>?)null!);

        // Act
        var actionResult = await _playersController.GetAllPlayersSortedByRank();

        // Assert
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = actionResult as NotFoundObjectResult;
        notFoundResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult?.Value.Should().Be(Messages.NotFoundMessages.PlayerNotFound);
        _mockPlayerService.Verify(service => service.GetAllPlayersSortedByRankAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllPlayersSortedByRank_WhenServiceThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var serviceException = new Exception("Service layer error during get all");
        _mockPlayerService.Setup(service => service.GetAllPlayersSortedByRankAsync())
                            .ThrowsAsync(serviceException);

        // Act
        var actionResult = await _playersController.GetAllPlayersSortedByRank();

        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var objectResult = actionResult as ObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult?.Value.Should().Be(Messages.ErrorMessages.UnexpectedError);
        _mockPlayerService.Verify(service => service.GetAllPlayersSortedByRankAsync(), Times.Once);
    }

    // Tests for GetPlayersStatistics()
    [Fact]
    public async Task GetPlayerStatistics_WhenStatisticsAvailable_ShouldReturnOkWithStatistics()
    {
        // Arrange
        var expectedStats = new PlayerStatisticsDto { AverageBmi = 22.5, MedianHeight = 180.0 };
        _mockPlayerStatisticsService.Setup(s => s.GetPlayersStatisticsAsync()).ReturnsAsync(expectedStats);

        // Act
        var result = await _playersController.GetPlayersStatistics();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.Value.Should().BeEquivalentTo(expectedStats);
    }
    
    [Fact]
    public async Task GetStatistics_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        _mockPlayerStatisticsService.Setup(s => s.GetPlayersStatisticsAsync()).ReturnsAsync((PlayerStatisticsDto)null!);

        // Act
        var result = await _playersController.GetPlayersStatistics();

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetStatistics_Returns500_WhenServiceThrowsException()
    {
        // Arrange
        _mockPlayerStatisticsService.Setup(s => s.GetPlayersStatisticsAsync()).ThrowsAsync(new Exception("Simulated error"));

        // Act
        var result = await _playersController.GetPlayersStatistics();

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be(Messages.ErrorMessages.UnexpectedErrorOccurredGetPlayersStatistics);
    }
}


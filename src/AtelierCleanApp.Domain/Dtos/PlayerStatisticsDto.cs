namespace AtelierCleanApp.Domain.Dtos;

/// <summary>
/// Represents a player statistics DTO object.
/// This class is used to transfer player statistics data between layers.
/// </summary>
public class PlayerStatisticsDto
{
    public string CountryWithHighestWinRatio { get; set; } = string.Empty;
    public double AverageBmi { get; set; }
    public double MedianHeight { get; set; }
}

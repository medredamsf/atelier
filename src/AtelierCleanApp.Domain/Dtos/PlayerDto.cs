namespace AtelierCleanApp.Domain.Dtos;

/// <summary>
/// Represents a player DTO object.
/// This class is used to transfer player statistics data between layers.
/// </summary>
public class PlayerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;

    public CountryDto Country { get; set; }
    public PlayerDataDto Data { get; set; }
}

namespace AtelierCleanApp.Domain.Entities;

/// <summary>
/// Represents player data in the system.
/// </summary>
public class Player
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;

    public Country Country { get; set; } = new();
    public PlayerData Data { get; set; } = new();
}

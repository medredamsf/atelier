namespace AtelierCleanApp.Domain.Entities;

/// <summary>
/// Represents a country in the system.
/// </summary>
public class Country
{
    public string Code { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;

    public ICollection<Player> Players { get; set; } = new List<Player>();
}
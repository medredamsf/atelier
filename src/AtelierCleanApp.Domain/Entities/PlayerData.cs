namespace AtelierCleanApp.Domain.Entities;

/// <summary>
/// Represents player data in the system.
/// This class contains additional information about a player.
/// It is linked to the Player entity through a one-to-one relationship.
/// </summary>
public class PlayerData
{
    public int PlayerId { get; set; }  // Acts as both PK and FK
    public int Rank { get; set; }
    public int Points { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Age { get; set; }
    public List<string> LastResults { get; set; } = new();

    public Player Player { get; set; }
}
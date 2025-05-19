namespace AtelierCleanApp.Domain.Dtos;

/// <summary>
/// Represents a player data DTO object.
/// This class is used to transfer player statistics data between layers.
/// </summary>
public class PlayerDataDto
{
    public int Rank { get; set; }
    public int Points { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Age { get; set; }

    public List<string> LastResults { get; set; } = new();
}



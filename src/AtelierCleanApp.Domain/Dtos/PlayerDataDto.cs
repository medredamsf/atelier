namespace AtelierCleanApp.Domain.Dtos;

public class PlayerDataDto
{
    public int Rank { get; set; }
    public int Points { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Age { get; set; }

    public List<string> LastResults { get; set; } = new();
}



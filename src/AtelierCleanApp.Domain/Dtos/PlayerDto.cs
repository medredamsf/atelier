namespace AtelierCleanApp.Domain.Dtos;

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

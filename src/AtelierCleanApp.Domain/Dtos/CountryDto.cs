namespace AtelierCleanApp.Domain.Dtos;

/// <summary>
/// Represents a country DTO object.
/// This class is used to transfer country data between layers.
/// </summary>
public class CountryDto
{
    public string Code { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
}
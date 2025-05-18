using AtelierCleanApp.Domain.Entities;

namespace AtelierCleanApp.Domain.Tests.Entities;

public class PlayerTests
{
    // Test 1: Can create a Player object with the default constructor.
    [Fact]
    public void Player_CanBeCreatedWithDefaultConstructor()
    {
        // Arrange & Act
        var player = new Player();

        // Assert
        Assert.NotNull(player);
        Assert.Equal(0, player.Id);
        Assert.Empty(player.FirstName);
        Assert.Empty(player.LastName);
        Assert.Empty(player.ShortName);
        Assert.Empty(player.Sex);
        Assert.Empty(player.CountryCode);
        Assert.Empty(player.Picture);
        Assert.NotNull(player.Country);
        Assert.NotNull(player.Data);
    }

    // Test 2: Can set and get properties of a Player object.
    [Fact]
    public void Player_PropertiesCanBeSetAndGet()
    {
        // Arrange
        var player = new Player();
        var testLastResults = "1,0,1";

        // Act
        player.Id = 1;
        player.FirstName = "Roger";
        player.LastName = "Federer";
        player.ShortName = "R.FED";
        player.Sex = "M";
        player.CountryCode = "SUI";
        player.Picture = "https://example.com/federer.png";
        player.Country = new Country
        {
            Code = "SUI",
            Picture = "https://example.com/switzerland.png"
        };
        player.Data = new PlayerData
        {
            PlayerId = 1,
            Rank = 2,
            Points = 2000,
            Weight = 85000,
            Height = 185,
            Age = 40,
            LastResults = testLastResults.Split(',').ToList()
        };

        // Assert
        Assert.Equal(1, player.Id);
        Assert.Equal("Roger", player.FirstName);
        Assert.Equal("Federer", player.LastName);
        Assert.Equal("R.FED", player.ShortName);
        Assert.Equal("M", player.Sex);
        Assert.Equal("SUI", player.CountryCode);
        Assert.Equal("https://example.com/federer.png", player.Picture);
        Assert.NotNull(player.Country);
        Assert.Equal("SUI", player.Country.Code);
        Assert.Equal("https://example.com/switzerland.png", player.Country.Picture);
        Assert.NotNull(player.Data);
        Assert.Equal(1, player.Data.PlayerId);
        Assert.Equal(2, player.Data.Rank);
        Assert.Equal(2000, player.Data.Points);
        Assert.Equal(85000, player.Data.Weight);
        Assert.Equal(185, player.Data.Height);
        Assert.Equal(40, player.Data.Age);
        Assert.Equal(testLastResults, string.Join(",", player.Data.LastResults));
    }
}

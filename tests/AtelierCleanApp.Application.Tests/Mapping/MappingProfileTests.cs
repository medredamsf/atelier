using AutoMapper;
using FluentAssertions;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Domain.Dtos;
using AtelierCleanApp.Application.Mappings;

namespace AtelierCleanApp.Application.Tests.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_Player_To_PlayerDto()
    {
        // Arrange
        var player = new Player
        {
            Id = 1,
            FirstName = "Rafael",
            LastName = "Nadal",
            ShortName = "R.NAD",
            Sex = "M",
            Picture = "https://example.com/nadal.png",
            Country = new Country
            {
                Code = "ESP",
                Picture = "https://example.com/spain.png"
            },
            Data = new PlayerData
            {
                PlayerId = 1,
                Rank = 1,
                Points = 3000,
                Weight = 85000,
                Height = 185,
                Age = 36,
                LastResults = new List<string> { "1", "0", "1", "1", "0" }
            }
        };

        // Act
        var dto = _mapper.Map<PlayerDto>(player);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(player.Id);
        dto.FirstName.Should().Be(player.FirstName);
        dto.Country.Code.Should().Be(player.Country.Code);
        dto.Data.LastResults.Should().BeEquivalentTo(player.Data.LastResults);
    }

    [Fact]
    public void Should_Map_PlayerData_To_PlayerDataDto()
    {
        // Arrange
        var data = new PlayerData
        {
            PlayerId = 1,
            Rank = 2,
            Points = 2500,
            Weight = 80000,
            Height = 180,
            Age = 30,
            LastResults = new List<string> { "1", "1", "0", "1", "0" }
        };

        // Act
        var dto = _mapper.Map<PlayerDataDto>(data);

        // Assert
        dto.Rank.Should().Be(data.Rank);
        dto.LastResults.Should().BeEquivalentTo(data.LastResults);
    }
}

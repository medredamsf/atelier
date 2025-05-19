using AutoMapper;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Domain.Dtos;

namespace AtelierCleanApp.Application.Mappings;

/// <summary>
/// Represents the mapping profile for AutoMapper.
/// This class defines the mappings between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Player, PlayerDto>();
        CreateMap<Country, CountryDto>();
        CreateMap<PlayerData, PlayerDataDto>();
    }
}
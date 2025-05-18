using AutoMapper;
using AtelierCleanApp.Domain.Entities;
using AtelierCleanApp.Domain.Dtos;

namespace AtelierCleanApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Player, PlayerDto>();
        CreateMap<Country, CountryDto>();
        CreateMap<PlayerData, PlayerDataDto>();
    }
}
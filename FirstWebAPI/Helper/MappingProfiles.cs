using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Models;

namespace FirstWebAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}

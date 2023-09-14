using AutoMapper;
using FirstWebAPI.Dto;
using FirstWebAPI.Models;

namespace FirstWebAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //could use ReverseMap() for reverse relationships
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<CreatePokemonDto, Pokemon>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<CreateReviewerDto, Reviewer>();
        }
    }
}

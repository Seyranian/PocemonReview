using AutoMapper;
using PocemonReview.DTO;
using PocemonReview.Models;

namespace PocemonReview.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDTO>();
            CreateMap<PokemonDTO, Pokemon>();
            CreateMap<Category,CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Country,CountryDTO>();
            CreateMap<CountryDTO, Country>();
            CreateMap<Owner,OwnerDTO>();
            CreateMap<OwnerDTO, Owner>();
            CreateMap<Review,ReviwDTO>();
            CreateMap<ReviwDTO, Review>();
            CreateMap<Reviewer,ReviewerDTO>();
            CreateMap<ReviewerDTO, Reviewer>();
        }
    }
}

using Api.Basic.Entities;
using Api.Basic.Models;
using AutoMapper;

namespace Api.Basic.Profiles
{
    public class PoiProfile : Profile
    {
        public PoiProfile()
        {
            CreateMap<Poi, PoiDto>();
            CreateMap<PoiForCreationDto, Poi>();
            CreateMap<PoiForUpdateDto, Poi>().ReverseMap();
           // CreateMap<Poi, PoiForUpdateDto>();
        }
    }
}

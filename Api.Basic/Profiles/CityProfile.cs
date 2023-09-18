using Api.Basic.Entities;
using Api.Basic.Models;
using AutoMapper;

namespace Api.Basic.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            //        Source - Destination 
            // Missing Property will be ignored
            // Same prop source to same pro destination
            CreateMap<City, CityWithoutPoiDto>();
            CreateMap<City, CityDto>();
        }
    }
}

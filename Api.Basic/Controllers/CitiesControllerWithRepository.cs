using System.Collections;
using Api.Basic.Models;
using Api.Basic.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
namespace Api.Basic.Controllers;


public class CitiesControllerWithRepository : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ICityInfoRepository _cityInfoRepository;

    public CitiesControllerWithRepository(ILogger<BaseApiController> logger,IMapper mapper
        ,ICityInfoRepository cityInfoRepository) : base(logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    }
  
  
   [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPoiDto>>> GetCities()
    {
        var cityEntity = await _cityInfoRepository.GetCitiesAsync();




        #region WithoutAutoMapper-Note10

        //var results = new List<CityWithoutPoiDto>();
        //foreach (var city in cityEntity)
        //{
        //    results.Add(new CityWithoutPoiDto
        //    {
        //        Id = city.Id,
        //        Description = city.Description,
        //        Name = city.Name

        //    });

        //} 




        #endregion

        #region Note10
        var result = _mapper.Map<IEnumerable<CityWithoutPoiDto>>(cityEntity);
    #endregion
        return Ok(result);

        // in this kind of methods we have only 200, 
        // as we know on get all it's ok the response would be null
    }

    [HttpGet("{id}")]
    
   //https://localhost:5001/api/CitiesControllerWithRepository/1?includePoi=true'
    public async Task<IActionResult> GetCity(int id,bool includePoi = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id,includePoi);
        if (city == null) return NotFound();

        
        if (includePoi) return Ok(_mapper.Map<CityDto>(city)); // Note this collection has 2 entity so we have to write mapping of them on profile!


        return Ok(_mapper.Map<CityWithoutPoiDto>(city));  // this approach is not correct despite of not getting error, because our out put is CityDto but we return CityWithoutPoiDto


        // so we change ActionResult<CityDto> into IActionResult
        // ActionResult => whatever





    }
}






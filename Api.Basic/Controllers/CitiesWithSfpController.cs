using System.Collections;
using System.Text.Json;
using Api.Basic.Models;
using Api.Basic.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Api.Basic.Controllers;

[Authorize] 
[ApiVersion("1.0")]
public class CitiesWithSfpController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ICityInfoRepository _cityInfoRepository;
    const int maxCitiesPageSize = 20;


    public CitiesWithSfpController(ILogger<BaseApiController> logger, IMapper mapper
        , ICityInfoRepository cityInfoRepository) : base(logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    }
    /// <summary>
    /// GetCities
    /// </summary>
    /// <param name="name">the name of city</param>
    /// <returns>An ActionResult</returns>

    [HttpGet]
    // everything would be queryString and no need to write [FromQuery]  tanks to just existing the name on entrance of a an action and not route!|| 
    // [FromQuery(Name = "filteroname")] string  name, name would be bounded to filteroname
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CityWithoutPoiDto>>> GetCities(string? name)
    {
        var filteredCityEntity = await _cityInfoRepository.GetCitiesAsyncFiletered(name);


        var result = _mapper.Map<IEnumerable<CityWithoutPoiDto>>(filteredCityEntity);

        return Ok(result);

        // in this kind of methods we have only 200, 
        // as we know on get all it's ok the response would be null
    }
    [HttpGet("/search/cities")] // note 11
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<ActionResult<IEnumerable<CityWithoutPoiDto>>> GetSearchedCities(string? searchQuery)
    {
        var searchedCityEntity = await _cityInfoRepository.GetCitiesAsyncSearched(searchQuery);


        var result = _mapper.Map<IEnumerable<CityWithoutPoiDto>>(searchedCityEntity);

        return Ok(result);

        // in this kind of methods we have only 200, 
        // as we know on get all it's ok the response would be null
    }
    [HttpGet("/paged/cities")] // note 11

    // pageNumber=1,int pageSize=10 these must have value! note-11
    public async Task<ActionResult<IEnumerable<CityWithoutPoiDto>>> GetPagedCities(string? name,
        string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }
        // tuple
        var (cityEntities, paginationMetadata) = await _cityInfoRepository
            .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
        Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(paginationMetadata));

        var result = _mapper.Map<IEnumerable<CityWithoutPoiDto>>(cityEntities);

        return Ok(result);

        // in this kind of methods we have only 200, 
        // as we know on get all it's ok the response would be null
    }
    /// <summary>
    /// GetCity
    /// </summary>
    /// <param name="id">The id of the city</param>
    /// <param name="includePoi"> whether or not to include the poi </param>
    /// <returns>An ActionResult</returns>
    /// <response code="200">Returns the requested city</response>
    /// <response code="404">Not Found!</response>

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    //https://localhost:5001/api/CitiesControllerWithRepository/1?includePoi=true'
    public async Task<IActionResult> GetCity(int id, bool includePoi = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePoi);
        if (city == null) return NotFound();


        if (includePoi) return Ok(_mapper.Map<CityDto>(city)); // Note this collection has 2 entity so we have to write mapping of them on profile!


        return Ok(_mapper.Map<CityWithoutPoiDto>(city));  // this approach is not correct despite of not getting error, because our out put is CityDto but we return CityWithoutPoiDto


        // so we change ActionResult<CityDto> into IActionResult
        // ActionResult => whatever





    }
}






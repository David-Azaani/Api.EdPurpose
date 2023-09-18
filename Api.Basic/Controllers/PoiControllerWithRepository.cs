
using Api.Basic.Entities;
using Api.Basic.Models;
using Api.Basic.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Api.Basic.Controllers;
[Authorize(Policy = "MustIran")]

[Route("api/cities/{cityId}/[controller]")] // to access to che child with route!
[ApiController] // check for validation automatically and 400 errors (ex :empty body)
public class PoiControllerWithRepository : ControllerBase

{
    private readonly ILogger<PoiControllerWithRepository> _logger;
    private readonly IMailService _localMailService;
    private readonly IMapper _mapper;
    private readonly ICityInfoRepository _cityInfoRepository;



    public PoiControllerWithRepository(ILogger<PoiControllerWithRepository> logger
        , IMailService mailService, IMapper mapper,
        ICityInfoRepository cityInfoRepository
       )
    {
        _logger = logger ??
                  throw new ArgumentNullException(nameof(logger));
        _localMailService = mailService ??
            throw new ArgumentNullException(nameof(mailService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

    }







    [HttpGet]
    public async Task<ActionResult<IEnumerable<PoiDto>>> GetPois(int cityId)
    {



        //var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

        //if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
        //{
        //    return Forbid();
        //}

        
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                     $"City with id {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }
        var cities = await _cityInfoRepository.GetPoisForCityAsync(cityId);



        return Ok(_mapper.Map<IEnumerable<PoiDto>>(cities));


    }

    [HttpGet("{poiId}", Name = "GetPoiGetPoiWithRepository")]
    public async Task<ActionResult<PoiDto>> GetPoi(int cityId, int poiId)
    {


        if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();


        var poi = await _cityInfoRepository.GetPoiForCityAsync(cityId, poiId);
        if (poi == null) return NotFound();

        return Ok(_mapper.Map<PoiDto>(poi));


    }

    [HttpPost]

    public async Task<ActionResult<PoiDto>> CreatePoi(int cityId, PoiForCreationDto poiForCreationDto)
    {

        if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();


        var creationObj = _mapper.Map<Poi>(poiForCreationDto);


        await _cityInfoRepository.AddPoiForCityAsync(cityId, creationObj);
        await _cityInfoRepository.SaveChangesAsync();// if we get error its 500 ,otherwise 201!
                                                     // so wi error the next line wouldn't be executed!
        var createdPoiToReturn = _mapper.Map<PoiDto>(creationObj);


        return CreatedAtRoute(nameof(GetPoi),
            new { cityId = cityId, poiId = createdPoiToReturn.Id },
            createdPoiToReturn);


    }


    [HttpPut("{poiId}")]

    public async Task<ActionResult> UpdatePoi(int cityId, int poiId, PoiForUpdateDto poiForUpdateDto)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();


        // find poi
        var poi = await _cityInfoRepository.GetPoiForCityAsync(cityId, poiId);
        if (poi == null) return NotFound();


        _mapper.Map(poiForUpdateDto, poi);
        await _cityInfoRepository.SaveChangesAsync();// be cause of tracking on ef just need save change!



        return NoContent(); //204 StatusCode
    }


    [HttpPatch("{poiId}")] // Partial Update!

    public async Task<ActionResult> PartiallyUpdatePoi(int cityId, int poiId,
        JsonPatchDocument<PoiForUpdateDto> poiPatchDocument)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();


        // find poi
        var poi = await _cityInfoRepository.GetPoiForCityAsync(cityId, poiId);
        if (poi == null) return NotFound();

        var poiToPatch = _mapper.Map<PoiForUpdateDto>(poi);



        poiPatchDocument.ApplyTo(poiToPatch, ModelState);
        if (!ModelState.IsValid) return BadRequest();

        if (!TryValidateModel(poiToPatch)) return BadRequest();


        _mapper.Map(poiToPatch, poi);
        await _cityInfoRepository.SaveChangesAsync();

        return NoContent();


    }

    [HttpDelete("{poiId}")] // full Update!

    public async Task<ActionResult> DeletePoi(int cityId, int poiId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();


        // find poi
        var poi = await _cityInfoRepository.GetPoiForCityAsync(cityId, poiId);
        if (poi == null) return NotFound();

        _cityInfoRepository.DeletePoi(poi);

           await _cityInfoRepository.SaveChangesAsync();

        _localMailService.Send(
               "Point of interest deleted.",
               $"Point of interest {poi.Name} with id {poi.Id} was deleted.");


        return NoContent();

    }

}



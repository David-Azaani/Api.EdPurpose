
using Api.Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Basic.Controllers;

[Route("api/cities/{cityId}/[controller]")] // to access to che child with route!
[ApiController] // check for validation automatically and 400 errors (ex :empty body)
public class PoiController : ControllerBase
{
    // [HttpGet("{cityId}")] because of using id on controller level we don't need to set cityId on http
    [HttpGet]
    public ActionResult<IEnumerable<PoiDto>> GetPois(int cityId)
    {

        var cities = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (cities==null)
        {
            return NotFound();
        }

        return Ok(cities.PoiCollection);


    }

    [HttpGet("{poiId}",Name = "GetPoi")]
    public ActionResult<PoiDto> GetPoi(int cityId,int poiId)
    {
        var cities = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (cities == null) { return NotFound();}

        var city = cities.PoiCollection.FirstOrDefault(a => a.Id == poiId);
        if (city == null) { return NotFound(); }

        return Ok(city);


    }

    [HttpPost]
    // [FromBody] is not necessary for getting from body
    // ... address =>... Poi and not CreatePoi
    public ActionResult<PoiDto> CreatePoi(int cityId, PoiForCreationDto poiForCreationDto)
    {



        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(); not necessary because of api [ApiController] 
        // so just write the anotation!
        //}



        var city = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (city == null) { return NotFound(); }

        var maxPoiId = CitiesDataStore.Current.Cities
            .SelectMany(a => a.PoiCollection).Max(p => p.Id);

        var finalPoi = new PoiDto()
        {
            Id = ++maxPoiId,
            Name = poiForCreationDto.Name,
            Description = poiForCreationDto.Description,
        };
        city.PoiCollection.Add(finalPoi);

        // for using CreatedAtRoute we have to set name to destination action!
        // return 201
        return CreatedAtRoute(nameof(GetPoi),
            new { cityId = cityId, poiId = finalPoi.Id },
            finalPoi);

        // when we are want to send and post data additional of data we should set content-type 
        // on header => content-type application/json => input(this input should be deserialized in body)
        // we see in header the value of location that refers to the address of this new item!
        //https://localhost:5001/api/cities/3/Poi/8
    }


}


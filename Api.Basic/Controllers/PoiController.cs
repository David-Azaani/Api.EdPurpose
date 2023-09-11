
using Api.Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Basic.Controllers;

[Route("api/cities/{cityId}/[controller]")] // to access to che child with route!
[ApiController]
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

    [HttpGet("{poiId}")]
    public ActionResult<PoiDto> GetPoi(int cityId,int poiId)
    {
        var cities = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (cities == null) { return NotFound();}

        var city = cities.PoiCollection.FirstOrDefault(a => a.Id == poiId);
        if (city == null) { return NotFound(); }

        return Ok(city);


    }

}


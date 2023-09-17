
using Api.Basic.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        if (cities == null)
        {
            return NotFound();
        }

        return Ok(cities.PoiCollection);


    }

    [HttpGet("{poiId}", Name = "GetPoi")]
    public ActionResult<PoiDto> GetPoi(int cityId, int poiId)
    {
        var cities = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (cities == null) { return NotFound(); }

        var city = cities.PoiCollection.FirstOrDefault(a => a.Id == poiId);
        if (city == null) { return NotFound(); }

        return Ok(city);


    }

    [HttpPost]
    // [FromBody] is not necessary for getting from body
    // ... address =>... Poi and not CreatePoi because of [Post]
    // be cause of cityid on this controller it's necessary to be here!
    // just writing [FromBody]is not necessary ! it's assumed by default!
    public ActionResult<PoiDto> CreatePoi(int cityId, PoiForCreationDto poiForCreationDto)
    {



        //if (!ModelState.IsValid)
        //{
        //    return BadRequest();        validation is not necessary because of api [ApiController] 
        //  so just write the Annotations!
        //}

        // this approach is not good for validation best is to use fluent validation!

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
        // return 201 or this 
        return CreatedAtRoute(nameof(GetPoi),
            new { cityId = cityId, poiId = finalPoi.Id },
            finalPoi);

        // when we are want to send and post data additional of data we should set content-type of what we are sending
        // on header => content-type application/json => input(this input should be deserialized in body)
        // we see in header the value of location that refers to the address of this new item!
        //https://localhost:5001/api/cities/3/Poi/8

        // during posting  item to save, if we set a property that doesn't exist on or creation class or dto
        // that will be ignored.
    }

    // Updating Operation :first likewise creating at first we need to create updateDto


    [HttpPut("{poiId}")] // full Update!
    // address :...:port/api/cities/cityId/poi/poiId
    public ActionResult UpdatePoi(int cityId, int poiId, PoiForUpdateDto poiForUpdateDto)
    {


        var city = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (city == null) { return NotFound(); };

        // find poi
        var poi = city.PoiCollection.FirstOrDefault(a => a.Id == poiId);
        if (poi == null) return NotFound();

        poi.Name = poiForUpdateDto.Name;
        poi.Description = poiForUpdateDto.Description;




        //Response return :
        //return NoContent(); 204 
        //return Ok();

        // and then  set content-type : application/json or whatever this should be set on every header's request
        // which we sent to our api because with this  being serialized and our server will know which format should be used to deserialized!

        // ***in this way the user should put all property and if  one property doesn't have value it'll be null so this way is for full update!
        return NoContent();
    }

    // patch for updating part of an entity at first we have to add microsoft.aspnetcore.jsonpatch
    // this is obligatory to have patch in our project!
    // the other one is microsoft.aspnetcore.mvc.newtonsoftjson
    // then do some configurations on program.cd file

    [HttpPatch("{poiId}")] // Partial Update!
    // address :...:port/api/cities/cityId/poi/poiId
    public ActionResult PartiallyUpdatePoi(int cityId, int poiId,
        JsonPatchDocument<PoiForUpdateDto> poiPatchDocument)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
        if (city == null) { return NotFound(); };

        // find poi
        var poi = city.PoiCollection.FirstOrDefault(a => a.Id == poiId);
        if (poi == null) { return NotFound(); }

        var poiPatch = new PoiForUpdateDto()
        {

            Name = poi.Name,
            Description = poi.Description,
        };

        poiPatchDocument.ApplyTo(poiPatch, ModelState);
        if (!ModelState.IsValid) return BadRequest();
        // on this approach we should check validation manually!

        if (!TryValidateModel(poiPatch))  return BadRequest(); //this is obligatory to check  after applying patchDocument! 

        poi.Name = poiPatch.Name;
        poi.Description = poiPatch.Description;
        return NoContent();

// review note-4 to see what's the response's body in this situation!

    }


}



using Api.Basic.Models;
using Microsoft.AspNetCore.Mvc;
namespace Api.Basic.Controllers;


public class CitiesController : BaseApiController
{
    public CitiesController(ILogger<BaseApiController> logger) : base(logger)
    {
    }
    // Commented because of note6
    //#region Comment
    //// api/[controller] maybe was no be a good approach because of future refactoring of controller name
    ////api/cities

    ////[HttpGet] // without template => api/cities (the route on controller name) on get request
    ///*[HttpGet("GetCities")]*/ // with template => api/cities/GetCities on get request
    //                           //public JsonResult GetCities()
    //                           //{
    //                           //    return new JsonResult(

    ////        //new List<object>
    ////        //{
    ////        //    new { id = 1,Name ="A"},
    ////        //    new { id = 2,Name="B"}
    ////        //    }
    ////        CitiesDataStore.Current.Cities
    ////    );

    ////    // adding status code to jsonresult  - badApproach!
    ////    //var temp = new JsonResult(CitiesDataStore.Current.Cities);
    ////    //temp.StatusCode = 200;
    ////    //return temp;

    ////} 
    //#endregion
    //[HttpGet]
    //public ActionResult<IEnumerable<CityDto>> GetCities()
    //{
    //    return Ok(CitiesDataStore.Current.Cities);

    //    // in this kind of methods we have only 200, 

    //}

    //[HttpGet("{id}")]
    //#region Comment
    //// .../api/Cities/id?id=2
    //// api/cities/1
    //// for working with param in routing we have to use {}

    ////public JsonResult GetCity(int id)
    ////{
    ////    return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == id));

    ////} 
    //#endregion
    //public ActionResult<CityDto> GetCity(int id)
    //{
    //    var result = CitiesDataStore.Current.Cities.FirstOrDefault(a => a.Id == id);
    //    if (result == null) return NotFound();
    //    return Ok(result);

    //    // if we don't specify this we'll get 200 even we don't have the specific record!

    //}
}



// common mistakes


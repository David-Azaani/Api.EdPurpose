
using Microsoft.AspNetCore.Mvc;

namespace Api.Basic.Controllers;

    [Route("api/[controller]")] // must be at control level
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> Logger;

        public BaseApiController(ILogger<BaseApiController> logger )
        {
            Logger = logger;
        }
    }

// ControllerBase : give us a lot of returning status code and helper methods! click to view source
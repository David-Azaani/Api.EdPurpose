
using Microsoft.AspNetCore.Mvc;

namespace Api.Basic.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> Logger;

        public BaseApiController(ILogger<BaseApiController> logger )
        {
            Logger = logger;
        }
    }


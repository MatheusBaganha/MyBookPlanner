using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyBookPlanner.WebApi.Controllers
{

    [ApiController]
    [Route("")]
    [AllowAnonymous]

    // Health Check API
    public class HomeController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "API Online"
            });
        }
    }
}

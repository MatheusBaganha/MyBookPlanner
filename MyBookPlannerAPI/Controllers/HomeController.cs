using Microsoft.AspNetCore.Mvc;

namespace MyBookPlannerAPI.Controllers
{

    [ApiController]
    [Route("")]

    // Health Check API
    // Verifica se a API está online ou não.
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

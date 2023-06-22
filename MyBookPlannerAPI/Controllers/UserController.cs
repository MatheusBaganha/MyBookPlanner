using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("/user/{id:int}")]
        public IActionResult GetById([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
               return NotFound();
            }
            return Ok(user);
        }
    }
}

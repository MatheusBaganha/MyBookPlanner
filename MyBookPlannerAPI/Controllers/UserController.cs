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
        public IActionResult GetUserById([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
               return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("/user")]
        public IActionResult PostUser([FromServices] CatalogDataContext context, [FromBody] User model)
        {
            try
            {
                var user = new User
                {
                    Id = 0,
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    Biography = "Olá. Estou usando o MyBookPlanner!",
                };

                context.Users.Add(user);
                context.SaveChanges();

                return Created($"/user/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

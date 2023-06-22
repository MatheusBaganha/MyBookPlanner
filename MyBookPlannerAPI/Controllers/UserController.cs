using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("/user")]
        public async Task<IActionResult> PostUserAsync([FromServices] CatalogDataContext context, [FromBody] User model)
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

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"/user/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> PutUserAsync([FromServices] CatalogDataContext context, [FromRoute] int id, [FromBody]  User model)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if(user == null) { 
                    return NotFound(); 
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.Biography = model.Biography;
                user.PasswordHash = model.PasswordHash;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Created($"/user/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if(user == null)
                {
                    return NotFound();
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

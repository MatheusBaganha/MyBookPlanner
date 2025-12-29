using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels;
using MyBookPlannerAPI.ViewModels.Users;
using SecureIdentity.Password;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return NotFound(new ResultViewModel<User>("User was not found."));
                }
                return Ok(new ResultViewModel<User>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<User>("Internal error."));
            }
        }

        [HttpPut]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> PutUserAsync([FromServices] CatalogDataContext context, [FromRoute] int id, [FromBody]  UpdateUserViewModel model)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if(user == null) { 
                    return NotFound(new ResultViewModel<User>("User was not found.")); 
                }

                var emailAlreadyInUse = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (emailAlreadyInUse != null)
                {
                    return StatusCode(401, new ResultViewModel<User>("This email adress is already in use."));
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.Biography = model.Biography;
                
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Created($"/user/{user.Id}", new ResultViewModel<User>(user));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<User>("It was not possible to update the user."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("Internal error."));
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

                return Ok(new ResultViewModel<User>(user));

            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ResultViewModel<User>("It was not possible to delete the user."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("Internal error."));
            }
        }
    }
}

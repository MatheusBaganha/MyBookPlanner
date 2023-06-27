using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.Services;
using MyBookPlannerAPI.ViewModels;
using MyBookPlannerAPI.ViewModels.Users;
using SecureIdentity.Password;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class UserAccountController : ControllerBase
    {

        [HttpPost]
        [Route("/user/login")]
        public async Task<IActionResult> Login([FromServices] TokenService tokenService, [FromServices] CatalogDataContext context, [FromBody] UserViewModel model)
        {
            try
            {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user == null)
                {
                    return StatusCode(401, new ResultViewModel<User>("User email or password is invalid."));
                }

                if(!PasswordHasher.Verify(user.PasswordHash, model.Password)) {
                    return StatusCode(401, new ResultViewModel<User>("User email or password is invalid."));
                }

                var token = tokenService.GenerateToken(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<User>("Internal error."));
            }
            
        }

        [HttpPost]
        [Route("/user/register")]
        public async Task<IActionResult> PostUserAsync([FromServices] CatalogDataContext context, [FromBody] RegisterViewModel model)
        {
            try
            {
                var userAlreadyExists = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (userAlreadyExists != null)
                {
                    return StatusCode(409, new ResultViewModel<User>("Someone using that email adress already exists."));
                }

                var user = new User
                {
                    // Values Id and Biography already defined.
                    // Id is generated automatically in DB and Biography is default value.
                    Id = 0,
                    Biography = "Olá. Estou usando o MyBookPlanner!",

                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = PasswordHasher.Hash(model.Password),
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"/user/{user.Id}", new ResultViewModel<User>(user));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<User>("It was not possible to create the user."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("Internal error."));
            }
        }
    }
}

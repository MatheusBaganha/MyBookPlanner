﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels.Users;

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
        public async Task<IActionResult> PostUserAsync([FromServices] CatalogDataContext context, [FromBody] RegisterViewModel model)
        {
            try
            {
                var user = new User
                {
                    // Values Id and Biography already defined.
                    // Id is generated automatically in DB and Biography is default value.
                    Id = 0,
                    Biography = "Olá. Estou usando o MyBookPlanner!",

                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = model.Password,
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"/user/{user.Id}", user);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "It was not possible to create the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal error.");
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
                    return NotFound(); 
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.Biography = model.Biography;
                user.PasswordHash = model.Password;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Created($"/user/{user.Id}", user);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "It was not possible to update the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal error.");
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
            catch (DbUpdateException ex)
            {
                return BadRequest("It was not possible to delete the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal error.");
            }
        }
    }
}

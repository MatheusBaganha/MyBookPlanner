using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserById(id);
            return user.ToActionResult();
        }

        [HttpPut]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody]  UpdateUserDTO model)
        {
            var user = await _userService.UpdateUser(id, model);
            return user.ToActionResult();
        }

        [HttpDelete]
        [Route("/user/{id:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _userService.DeleteUser(id);
            return user.ToActionResult();
        }
    }
}

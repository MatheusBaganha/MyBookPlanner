using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Authorize(Policy = "SameUser")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("/user/{idUser:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int idUser)
        {
            var user = await _userService.GetUserById(idUser);
            return user.ToActionResult();
        }

        [HttpPut]
        [Route("/user/{idUser:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int idUser, [FromBody]  UpdateUserDTO model)
        {
            var user = await _userService.UpdateUser(idUser, model);
            return user.ToActionResult();
        }

        [HttpDelete]
        [Route("/user/{idUser:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int idUser)
        {
            var user = await _userService.DeleteUser(idUser);
            return user.ToActionResult();
        }
    }
}

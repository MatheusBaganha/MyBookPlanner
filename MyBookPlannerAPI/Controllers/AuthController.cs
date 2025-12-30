using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/user/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            var login = await _authService.Login(userLogin);
            return login.ToActionResult();
        }

        [HttpPost]
        [Route("/user/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userRegister)
        {
            var register = await _authService.Register(userRegister);
            return register.ToActionResult();
        }
    }
}

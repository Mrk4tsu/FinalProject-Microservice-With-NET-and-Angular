using FN.Application.System.User;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FN.UserService.Controllers
{
    [Route("api/user")]
    [ApiController, AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var result = await _userService.Register(register);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _userService.Authenticate(login);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}

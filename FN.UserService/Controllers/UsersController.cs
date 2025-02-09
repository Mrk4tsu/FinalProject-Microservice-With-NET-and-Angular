using FN.Application.System.User;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FN.UserService.Controllers
{
    [Route("api/user")]
    [ApiController, Authorize]
    public class UsersController : BasesController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var result = await _userService.Register(register);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _userService.Authenticate(login);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("list-device")]
        public async Task<IActionResult> ListDevice()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();
            var result = await _userService.ListDevice(userId.Value);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("revoke-device")]
        public async Task<IActionResult> RevokeDevice(string clientId)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var request = new TokenRequest
            {
                UserId = userId.Value,
                ClientId = clientId
            };
            var result = await _userService.RevokeDevice(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("refresh-token"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _userService.RefreshToken(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}

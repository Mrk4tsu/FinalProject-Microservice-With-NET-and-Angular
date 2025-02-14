using FN.Application.Systems.User;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FN.UserService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthsController : BasesController
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public AuthsController(IUserService userService, IAuthService authService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var result = await _authService.Register(register);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _authService.Authenticate(login, HttpContext);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("logout"), AllowAnonymous]
        public async Task<IActionResult> Logout(TokenRequest request)
        {
            var result = await _authService.RevokeDevice(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("devices")]
        public async Task<IActionResult> ListDevice()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();
            var result = await _authService.GetRegisteredDevices(userId.Value);
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
            var result = await _authService.RevokeDevice(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("refresh-token"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _authService.RefreshToken(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}

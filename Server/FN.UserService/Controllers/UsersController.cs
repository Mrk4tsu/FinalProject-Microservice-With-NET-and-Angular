﻿using FN.Application.Systems.Redis;
using FN.Application.Systems.User;
using FN.Utilities;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FN.UserService.Controllers
{
    [Route("api/user")]
    [ApiController, Authorize]
    public class UsersController : BasesController
    {
        private readonly IUserService _userService;
        private readonly IRedisService _redisService;
        public UsersController(IUserService userService, IRedisService redisService)
        {
            _redisService = redisService;
            _userService = userService;
        }
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get(int userId)
        {
            var result = await _userService.GetById(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("confirm"), AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailChange(UpdateEmailResponse response)
        {
            var result = await _userService.ConfirmEmailChange(response);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("request")]
        public async Task<IActionResult> RequestUpdateMail(string newEmail)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();
            var requestResult = await _userService.RequestUpdateMail(userId.Value, newEmail);
            await _redisService.Publish(SystemConstant.MESSAGE_UPDATE_EMAIL_EVENT, new UpdateEmailResponse
            {
                UserId = userId.Value,
                NewEmail = newEmail,
                Token = requestResult.Data!
            });
            if (!requestResult.Success)
                return BadRequest(requestResult);
            return Ok(requestResult);
        }
        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateAvatar(IFormFile file)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();
            var result = await _userService.UpdateAvatar(userId.Value, file);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}

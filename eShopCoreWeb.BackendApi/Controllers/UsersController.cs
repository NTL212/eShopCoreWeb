﻿using eShopCoreWeb.Application.System.Users;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopCoreWeb.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromForm] LoginRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.Authenticate(request);
            if(string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Username or password is incorrect");
            }
            return Ok(new {token = resultToken});
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultRegis = await _userService.Register(request);
            if (!resultRegis)
            {
                return BadRequest("Register is unsuccessful.");
            }
            return Ok();
        }
    }
}

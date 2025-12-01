using Entities.Dtos;
using Entities.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public AccountController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] AccountDtoForLogin accountDto)
        {
            if (!await _manager.AccountService.LoginUserAsync(accountDto))
            {
                return Unauthorized();
            }

            var tokenDto = await _manager.AccountService.CreateTokenAsync(populateExp: true, rememberMe: accountDto.RememberMe);

            _manager.AccountService.SetTokensInsideCookie(tokenDto, HttpContext);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userInfo = await _manager.AccountService.GetCurrentUserInfoAsync(userId!);

            return Ok(userInfo);
        }

        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] AccountDtoForRegistration accountDto)
        {
            var result = await _manager.AccountService.RegisterUserAsync(accountDto);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            try
            {
                var tokenDto = await _manager.AccountService.RefreshTokenAsync(refreshToken);

                _manager.AccountService.SetTokensInsideCookie(tokenDto, HttpContext);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userInfo = await _manager.AccountService.GetCurrentUserInfoAsync(userId!);

                return Ok(userInfo);
            }
            catch (RefreshTokenBadRequestException)
            {
                HttpContext.Response.Cookies.Delete("accessToken");
                HttpContext.Response.Cookies.Delete("refreshToken");
                return Unauthorized();
            }
        }

        [HttpGet("check-auth")]
        [Authorize]
        public async Task<IActionResult> CheckAuth()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Unauthorized();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userInfo = await _manager.AccountService.GetCurrentUserInfoAsync(userId!);

            return Ok(userInfo);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("accessToken");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _manager.AccountService.InvalidateRefreshTokenAsync(userId!);

            return Ok();
        }
    }
}

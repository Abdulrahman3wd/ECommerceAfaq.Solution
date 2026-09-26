using ECommerceAfaq.Application.DTOs.Auth;
using ECommerceAfaq.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAfaq.API.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]  // api/auth/register 
         public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            try
            {
                var result =  await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]  // api/auth/login 
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string token, [FromBody] string newPassword)
        {
            try
            {
                await _authService.ResetPasswordAsync(email, token, newPassword); 
                return Ok(new {message = "password has been reset successfully."});
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            await _authService.ForgotPasswordAsync(email);
            return Ok( new {message = "If email registerd , a reset link /token has been generated"});
        }






    }
}

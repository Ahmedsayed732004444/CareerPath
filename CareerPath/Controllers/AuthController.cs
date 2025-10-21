using CareerPath.Contracts.Requests;
using CareerPath.Extensions;
using CareerPath.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CareerPath.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices _authServices) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var result = await _authServices.Login(request);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("Regester")]
        public async Task<IActionResult> RegesterAsync([FromBody] RegesterRequest request)
        {
            var result = await _authServices.RegesterAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var result = await _authServices.RefreshTokenAsync(request);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("ReSendConfirmationEmail")]
        public async Task<IActionResult> ReSendConfirmationEmailAsync([FromBody] ReSendConfirmationEmailRequest request)
        {
            var result = await _authServices.ReSendConfirmationEmailAsync(request);
            return result.IsSuccess ? Ok(result) : result.ToProblem();
        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
        {
            var result = await _authServices.ConfirmEmailAsync(request);
            return result.IsSuccess ? Ok(result) : result.ToProblem();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request)
        {
            var result = await _authServices.LogoutAsync(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("SendResetPasswordToken")]
        public async Task<IActionResult> SendResetPasswordTokenAsync([FromBody] SendResetPasswordRequest request)
        {
            var result = await _authServices.SendResetPasswordTokenAsync(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var result = await _authServices.ResetPasswordAsync(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackOffice.Application.Users;
using System;

namespace BackOffice.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserActivationService _userActivationService;

        public AuthController(UserActivationService userActivationService)
        {
            _userActivationService = userActivationService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            string clientId = "1066153135876-ddrj4ms6r801m49cjun24ut057p95m18.apps.googleusercontent.com";
            string redirectUri = "http://localhost:5184/auth/callback";
            string authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope=email&access_type=offline";

            return Redirect(authorizationUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> AuthCallback(string code)
        {
            try
            {
                var result = await _userActivationService.HandleOAuthCallbackAsync(code);
                
                if (result.IsUserActive)
                {
                    // Redirect to the main page if the user is active
                    return Redirect("http://localhost:5184/swagger/index.html");
                }
                else
                {
                    // Return Unauthorized if the user is inactive
                    return Unauthorized(new { success = false, message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateAccount(string token)
        {
            try
            {
                await _userActivationService.ActivateUserAsync(token);
                return Ok(new { success = true, message = "Account activated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("send-activation")]
        public async Task<IActionResult> SendActivationEmail(string email)
        {
            try
            {
                await _userActivationService.SendActivationEmailAsync(email);
                return Ok(new { success = true, message = "Activation email sent." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
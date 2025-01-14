using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackOffice.Application.Users;
using BackOffice.Domain.Users;
using BackOffice.Application.OAuth; 

namespace BackOffice.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserActivationService _userActivationService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(UserActivationService userActivationService, JwtTokenService jwtTokenService, UserService userService, IConfiguration configuration)
        {
            _userActivationService = userActivationService;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _configuration = configuration; 
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            string clientId = _configuration["GoogleIAM:ClientId"];
            string redirectUri = "https://api-dotnet.hospitalz.site/api/v1/auth/callback";
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
                    var token = _jwtTokenService.GenerateToken(result.Email, result.Role);
                    Console.WriteLine(token);
                    var redirect = $"https://hospitalz.site/auth/callback?token={token}";
                    Console.WriteLine(redirect); // Log the redirect URL
                    return Redirect(redirect);
                }
                else
                {
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

        
    [HttpPost("send-conf-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserDto userDto)
    {
        try
        {
            var registeredUser = await _userActivationService.RegisterUserAsync(userDto.Role,userDto.PhoneNumber,userDto.FirstName,userDto.LastName,userDto.FullName);
            return Ok(new { success = true, user = registeredUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
    }
}

using WebApp.DB;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace WebApp.Controllers
{
    /// <summary>
    /// Контроллер для пользователя
    /// </summary>
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public UserController(ILogger logger, IUserService userService, Context context, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials(wrong email");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userService.GetUserByEmail(request.Email);

            if (user is null)
                return Unauthorized();

            return Ok(await _userService.Authenticate(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(request);

            var registrationRequest = await _userService.Registration(request);
            //if (!await _roleManager.RoleExistsAsync(RoleConstants.Member))
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Member));
            //}
            if (registrationRequest is null)
                return BadRequest(request);
            else if (registrationRequest is AuthenticationRequest ar)
                return await Authenticate(ar);

            return BadRequest(request);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;
            var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _configuration.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }
    }
}

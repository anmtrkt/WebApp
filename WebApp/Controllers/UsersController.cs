using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using WebApp.DB;
using WebApp.Extensions;
using WebApp.Migrations;
using WebApp.Models;
using WebApp.Models.UsersModel;
using WebApp.Services.EmailServices;
using WebApp.Services.UserServices;
using WebApp.Web;

namespace WebApp.Controllers
{
    /// <summary>
    /// Контроллер для пользователя
    /// </summary>
    public class UserController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public UserController(IEmailService emailService, RoleManager<IdentityRole<Guid>> roleManager, ILogger logger, IUserService userService, Context context, UserManager<User> userManager, IConfiguration configuration)
        {
            _emailService = emailService;
            _roleManager = roleManager;
            _logger = logger;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        /// <summary>
        /// Аутентификация
        /// </summary>
        /// <param name="AuthenticationRequest"></param>>
        /// <remarks>
        /// Контроллер для аутентификации
        /// </remarks>
        /// <returns>AuthResponse</returns>
        [HttpPost]
        [Route(Routes.LoginRoute)]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userService.GetUserByEmail(request.Email);

            if (user is null)
                return Unauthorized();


            var temp = await _userService.Authenticate(user);
            HttpContext.Response.Cookies.Append("test", temp.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
            return Ok(temp);
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="RegisterRequest"></param>>
        /// <remarks>
        /// Регистрация нового пользователя
        /// </remarks>
        /// <returns>AuthResponse</returns>
        [HttpPost]
        [Route(Routes.RegistrationRoute)]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(request);

            var registrationRequest = await _userService.Registration(request);
            if (!await _roleManager.RoleExistsAsync(RoleConstants.Administrator))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.Moderator));
            }
            if (registrationRequest is null)
                return BadRequest(request);
            else if (registrationRequest is AuthenticationRequest ar)
            {
                var mail = await _emailService.RegisterMail(request.Email, $"{request.FirstName} {request.MiddleName}");
                if (!mail) {
                    return BadRequest("something went wrong");
                }
                return await Authenticate(ar);
            }

            return BadRequest(request);
        }

        /// <summary>
        /// Рефреш токен
        /// </summary>
        /// <param name="ObjectResult"></param>>
        /// <remarks>
        /// Рефреш токен чзх
        /// </remarks>
        /// <returns>ObjectResult</returns>
        [HttpPost]
        [Route(Routes.RefreshRoute)]
        [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
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

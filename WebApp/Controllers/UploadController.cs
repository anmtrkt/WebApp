using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using WebApp.DB;
using WebApp.Extensions;
using WebApp.Migrations;
using WebApp.Models.UsersModel;
using WebApp.Services.UserServices;
using WebApp.Web;

namespace WebApp.Controllers
{
    /// <summary>
    /// Контроллер для пользователя
    /// </summary>
    public class UploadController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
        [Authorize(Roles = $"{RoleConstants.Administrator},{RoleConstants.Moderator}")]
        [Route(Routes.UploadHotelPhotoRoute)]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadHotelPhoto(int Name, IFormFile Image)
        {
            string filePath = Path.Combine("static", "images", $"{Name}.webp");
            using (var stream = System.IO.File.Create(filePath))
            {
                await Image.CopyToAsync(stream);
            }
            return Ok();

        }

    }
}

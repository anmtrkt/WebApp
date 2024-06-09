using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Services.AdminServices;
using WebApp.Services.BookingServices;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Net;
using WebApp.Web;
using WebApp.Models.UsersModel;

namespace WebApp.Controllers
{

    namespace WebApp.Controllers
    {
        public class AdminController : Controller
        {
            private readonly ILogger _logger;
            private readonly IAdminService _adminService;
            private readonly Context _context;
            private readonly UserManager<User> _userManager;
            private readonly IConfiguration _configuration;
            public AdminController(ILogger logger, IAdminService adminService, Context context, UserManager<User> userManager, IConfiguration configuration)
            {
                _logger = logger;
                _adminService = adminService;
                _context = context;
                _userManager = userManager;
                _configuration = configuration;
            }
            /// <summary>
            /// Выдача роли
            /// </summary>
            /// <param name="UserId, int"></param>>
            /// <remarks>
            /// Выдача роли пользователю(1- адм, 2- модер, 3- мемб)
            /// </remarks>
            /// <returns>bool</returns>
            [HttpPut]
            [Authorize(Roles = RoleConstants.Administrator)]
            [Route(Routes.RoleRoute)]
            [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.OK)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<IActionResult> RoleAdding(Guid UserId, int role)
            {
                var result = await _adminService.RoleMaking(UserId, role);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Что-то пошло не так");
                }
            }
            /// <summary>
            /// Все пользователи
            /// </summary>
            /// <param name=""></param>>
            /// <remarks>
            /// Все зарегистрированные пользователи 
            /// </remarks>
            /// <returns>UserModel</returns>
            [HttpGet]
            [Authorize(Roles = RoleConstants.Administrator)]
            [Route(Routes.AllUsersRoute)]
            [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.OK)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<List<UserModel>> AllUsers()
            {
                var result = await _adminService.AllUsers();
                return result;
            }
            /// <summary>
            /// Пользователь по айди
            /// </summary>
            /// <param name="UserId"></param>>
            /// <remarks>
            /// Зарегистрированный пользователь по айди
            /// </remarks>
            /// <returns>UserModel</returns>
            [HttpGet]
            [Authorize(Roles = RoleConstants.Administrator)]
            [Route(Routes.OneUserByIdRoute)]
            [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.OK)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<object> OneUserById(Guid UserId)
            {
                var result = await _adminService.OneUserById(UserId);
                return result;
            }
            /// <summary>
            /// Пользователь по Email
            /// </summary>
            /// <param name="Email"></param>>
            /// <remarks>
            /// Зарегистрированный пользователь по Email
            /// </remarks>
            /// <returns>UserModel</returns>
            [HttpGet]
            [Authorize(Roles = RoleConstants.Administrator)]
            [Route(Routes.OneUserByEmailRoute)]
            [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.OK)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<object> OneUserByEmail(string Email)
            {
                var result = await _adminService.OneUserByEmail(Email);
                return result;
            }
            /// <summary>
            /// Удаление пользователя
            /// </summary>
            /// <param name="UserId"></param>>
            /// <remarks>
            /// Удаление пользователя из БД
            /// </remarks>
            /// <returns>bool</returns>
            [HttpDelete]
            [Authorize(Roles = RoleConstants.Administrator)]
            [Route(Routes.DeleteUserRoute)]
            [ProducesResponseType(typeof(ObjectResult), (int)HttpStatusCode.OK)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<bool> DeleteUser(Guid UserId)
            {
                var result = await _adminService.DeleteUser(UserId);
                return result;
            }
        }
    }
}

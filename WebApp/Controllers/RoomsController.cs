using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Services.RoomServices;
using WebApp.Services.BookingServices;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Net;
using WebApp.Web;
using WebApp.Models.RoomsModel;
using WebApp.Models.UsersModel;

namespace WebApp.Controllers
{
    public class RoomsController: Controller
    {
        private readonly ILogger _logger;
        private readonly IRoomService _roomService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public RoomsController( ILogger logger, IRoomService roomService, Context context, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _roomService = roomService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Все комнаты из БД
        /// </summary>
        /// <param name=""></param>>
        /// <remarks>
        /// Получение всех комнат
        /// </remarks>
        /// <returns>Список комнат</returns>
        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.Moderator}, {RoleConstants.Administrator}")]
        [Route(Routes.AllRoomsRoute)]
        [ProducesResponseType(typeof(List<Room>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllRooms()
        {
            var AllRooms = await _roomService.GetAllRooms();
            return Ok(AllRooms);
        }
        /// <summary>
        /// Создание комнаты
        /// </summary>
        /// <param name="CreateRoomRequest"></param>>
        /// <remarks>
        /// Контроллер для создания комнаты
        /// </remarks>
        /// <returns>Room</returns>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Moderator}, {RoleConstants.Administrator}")]
        [Route(Routes.CreateRoomRoute)]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
         public async Task<IActionResult> CreateRoom(CreateRoomRequest request)
        {
            return Ok(await _roomService.CreateRoom(request.Id, request.HotelId, request.Name, request.Description, request.Price, request.Services, request.Quantity, request.ImageId));
        }
        /// <summary>
        /// Удаление комнаты
        /// </summary>
        /// <param name="RoomId"></param>>
        /// <remarks>
        /// Контроллер для удаления комнаты
        /// </remarks>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = $"{RoleConstants.Moderator}, {RoleConstants.Administrator}")]
        [Route(Routes.DeleteRoomRoute)]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRoom(int RoomId)
        {
            return Ok(await _roomService.DeleteRoom(RoomId));
        }
        /// <summary>
        /// Просто комната
        /// </summary>
        /// <param name="RoomId"></param>>
        /// <remarks>
        /// Контроллер для получения одной комнаты
        /// </remarks>
        /// <returns>Room</returns>
        [HttpGet]
        [Route(Routes.JustRoomRoute)]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> JustRoom(int RoomId)
        {
            var temp = await _roomService.JustRoom(RoomId);
            if (temp != null)
                return Ok(temp);
            else return BadRequest("Что-то пошло не так");
        }
        /// <summary>
        /// Поиск комнаты по сервисам
        /// </summary>
        /// <param name="string, int, int"></param>>
        /// <remarks>
        /// Контроллер для поиска подходящей комнаты
        /// </remarks>
        /// <returns>Room</returns>
        [HttpGet]
        [Route(Routes.FindRoomRoute)]
        [ProducesResponseType(typeof(Room), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindRoom(string Services, int MinPrice, int MaxPrice)
        {
            return Ok(await _roomService.FindRoom(Services, MinPrice, MaxPrice));
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Services.HotelServices;
using WebApp.Services.BookingServices;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Net;
using WebApp.Web;
using WebApp.Models.HotelsModel;
using WebApp.Models.UsersModel;

namespace WebApp.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHotelService _hotelService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public HotelsController(ILogger logger, IHotelService hotelService, Context context, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _hotelService = hotelService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        /// <summary>
        /// Получение всех отелей из БД
        /// </summary>
        /// <param name=" "></param>>
        /// <remarks>
        /// Получение всех отелей напрямую из БД(для админа)
        /// </remarks>
        /// <returns>GetHotelResponse</returns>
        [HttpGet]
        [Route(Routes.AllHotelsRoute)]
        [ProducesResponseType(typeof(List<GetHotelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllHotels()
        {
            var AllHotels = await _hotelService.GetAllHotels();
            return Ok(AllHotels);
        }
        /// <summary>
        /// Получение одного отеля из БД
        /// </summary>
        /// <param name="HotelId"></param>>
        /// <remarks>
        /// Получение одного отеля
        /// </remarks>
        /// <returns>GetHotelResponse</returns>
        [HttpGet]
        [Route(Routes.OneHotelRoute)]
        [ProducesResponseType(typeof(List<GetHotelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOneHotel(int HotelId)
        {
            var Hotel = await _hotelService.GetOneHotel(HotelId);
            return Ok(Hotel);
        }
        /// <summary>
        /// Получение всех отелей по локации, сервисам и звездам из БД
        /// </summary>
        /// <param name="Location, Services, Stars"></param>>
        /// <remarks>
        /// Получение всех подходящих
        /// </remarks>
        /// <returns>GetHotelResponse</returns>
        [HttpGet]
        [Route(Routes.FindByLocationRoute)]
        [ProducesResponseType(typeof(List<GetHotelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindByLocation(string Location, string? Services, int? Stars)
        {
            if (Location == null)
            {
                return BadRequest("Введите адрес");
            }
            if (Stars != null && Stars.GetType() != typeof(int))
            {
                return BadRequest("STARS MUST BE INT");
            }
            var AllHotels = await _hotelService.GetHotelsByLocationAndServices(Location, Services, Stars);
            return Ok(AllHotels);
        }
        /// <summary>
        /// Получение всех комнат отеля
        /// </summary>
        /// <param name="HotelId"></param>>
        /// <remarks>
        /// Получение всех комнат отеля по айди отелю
        /// </remarks>
        /// <returns>Room</returns>
        /// 

        [HttpGet]
        [Authorize]
        [Route(Routes.AllHotelRoomsRoute)]
        [ProducesResponseType(typeof(List<Room?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHotelRooms(int HotelId)
        {
            if (HotelId.GetType() != typeof(int))
            {
                return BadRequest("Bad Credentials");
            }
            return Ok(await _hotelService.GetHotelRooms(HotelId));
        }
        /// <summary>
        /// Создание отеля
        /// </summary>
        /// <param name="CreateHotelRequest"></param>>
        /// <remarks>
        /// Создание отеля(админ)
        /// </remarks>
        /// <returns>GetHotelResponse</returns>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Moderator}, {RoleConstants.Administrator}")]
        [Route(Routes.CreateHotelRoute)]
        [ProducesResponseType(typeof(Hotel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateHotel(CreateHotelRequest request)
        {
            return Ok(await _hotelService.CreateHotel(request.Id, request.Name, request.Location, request.RoomsQuantity, request.ImageId, request.Services, request.Stars));
        }
        /// <summary>
        ///  Удаление отеля
        /// </summary>
        /// <param name="HotelId"></param>>
        /// <remarks>
        /// Удаление отеля(админ)
        /// </remarks>
        /// <returns>bool</returns>
        [HttpDelete]
        [Authorize(Roles = $"{RoleConstants.Moderator}, {RoleConstants.Administrator}")]
        [Route(Routes.DeleteHotelRoute)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteHotel(int HotelId)
        {
            return Ok(await _hotelService.DeleteHotel(HotelId));
        }
    }
}

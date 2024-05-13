using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;
using WebApp.Services.RoomServices;
using WebApp.Services.BookingServices;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

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
        [HttpGet("Get All Rooms"), Authorize]
        public async Task<IActionResult> GetAllRooms()
        {
            var AllRooms = await _roomService.GetAllRooms();
            return Ok(AllRooms);
        }
        [HttpGet("Get Hotel Rooms"), Authorize]
        public async Task<IActionResult> GetHotelRooms(int HotelId)
        {
            return Ok(await _roomService.GetHotelRooms(HotelId));
        }
        [HttpPost("Create Room"), Authorize]
        public async Task<IActionResult> CreateRoom(int Id, int HotelId, string Name, string? Description, int Price, string Services, int Quantity, int ImageId)
        {
            return Ok(await _roomService.CreateRoom(Id, HotelId, Name, Description, Price, Services, Quantity, ImageId));
        }
    }
}

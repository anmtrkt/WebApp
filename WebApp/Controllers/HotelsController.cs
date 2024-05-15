using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;
using WebApp.Services.HotelServices;
using WebApp.Services.BookingServices;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

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
        [HttpGet("Get All Hotels"), Authorize]
        public async Task<IActionResult> GetAllRooms()
        {
            var AllHotels = await _hotelService.GetAllHotels();
            return Ok(AllHotels);
        }
        [HttpPost("Create Hotel"), Authorize]
        public async Task<IActionResult> CreateHotel(CreateHotelRequest request)
        {
            return Ok(await _hotelService.CreateHotel(request.Id, request.Name, request.Location, request.RoomsQuantity, request.ImageId, request.Services, request.Stars));
        }
        [HttpDelete("Delete Hotel"), Authorize]
        public async Task<IActionResult> DeleteHotel(int HotelId)
        {
            return Ok(await _hotelService.DeleteHotel(HotelId));
        }
    }
}

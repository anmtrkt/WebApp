using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;
using WebApp.Services.UserServices;
using WebApp.Services.BookingServices;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    public class BookingsController: Controller
    {
        private readonly ILogger _logger;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public BookingsController(ILogger logger, IBookingService bookingService, IUserService userService, Context context, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _bookingService = bookingService;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        [HttpPost("Get Bookings"), Authorize]
        public async Task<IActionResult> GetUserBookings()

        {
            var CurrentUser = _userManager.GetUserAsync(User).Result;
            return Ok(await _bookingService.GetUserBookings(CurrentUser.Email));
        }
        [HttpPost("Create Booking"), Authorize]
        public async Task<IActionResult> CreateUserBookings([FromBody] CreateBookingRequest request)
        {
            var CurrentUser = _userManager.GetUserAsync(User).Result;
            return Ok(await _bookingService.CreateBooking(request.RoomId, request.HotelId, CurrentUser,request.DateFrom, request.DateTo));
        }
    }
}

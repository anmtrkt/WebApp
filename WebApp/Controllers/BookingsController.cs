using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;
using WebApp.Services.UserServices;
using WebApp.Services.BookingServices;

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

        [HttpPost("Get Bookings")]
        public async Task<IActionResult> GetUserBookings([FromBody] GetBookingRequest request)
        {
            return Ok(await _bookingService.GetUserBookings(request.Email));
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Web;
using WebApp.Models;
using WebApp.Services.UserServices;
using WebApp.Services.BookingServices;
using WebApp.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using WebApp.Services.EmailServices;
using WebApp.Models.BookingsModel;
using Hangfire;

namespace WebApp.Controllers
{
    /// <summary>
    /// Контроллер для букинга
    /// </summary>
    public class BookingsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public BookingsController(IEmailService emailService, ILogger logger, IBookingService bookingService, IUserService userService, Context context, UserManager<User> userManager, IConfiguration configuration)
        {
            _emailService = emailService;
            _logger = logger;
            _bookingService = bookingService;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        /// <summary>
        /// Получение всех букингов юзера
        /// </summary>
        /// <param name=" "></param>>
        /// <remarks>
        /// Получение букингов аутентифицированного пользователья
        /// </remarks>
        /// <returns>Список букингов</returns>
        [HttpGet]
        [Authorize]
        [Route(Routes.AllUserBookingsRoute)]
        [ProducesResponseType(typeof(List<GetBookingResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserBookings()

        {
            var CurrentUser = _userManager.GetUserAsync(User).Result;
            if (CurrentUser == null)
            {
                return BadRequest(HttpStatusCode.InternalServerError);
            }
            
            return Ok(await _bookingService.GetUserBookings(CurrentUser.Email));
        }
        /// <summary>
        /// Создание нового букинга
        /// </summary>
        /// <param name="CreateBookingRequest"></param>>
        /// <remarks>
        /// Создание нового букинга
        /// </remarks>
        /// <returns>Созданный букинг</returns>
        [HttpPost]
        [Authorize]
        [Route(Routes.NewBookingRoute)]
        [ProducesResponseType(typeof(GetBookingResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateUserBookings([FromBody] CreateBookingRequest request)
        {
            var CurrentUser = _userManager.GetUserAsync(User).Result;
            if (request.DateTo.CompareTo(request.DateFrom) < 0) return BadRequest("Bad credentials");

            
            var booking = await _bookingService.CreateBooking(request.RoomId, request.HotelId, CurrentUser, request.DateFrom, request.DateTo);
            var SendMail = _emailService.BookingMail(CurrentUser.Email, CurrentUser.Name, booking);
            var temp = await _emailService.Reminder(request.DateFrom, CurrentUser.Email, CurrentUser.Name, booking);

            return Ok(booking);
        }
        /// <summary>
        /// Удаление букинга
        /// </summary>
        /// <param name="GetBookingResponse"></param>>
        /// <remarks>
        /// Удаление существующего букинга
        /// </remarks>
        /// <returns>bool</returns>
        [HttpDelete]
        [Authorize]
        [Route(Routes.DeleteBookingRoute)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteBooking(Guid BookingId)
        {
            return Ok(await _bookingService.DeleteUserBooking(BookingId));
        }
        /// <summary>
        /// Получение букинга по айди
        /// </summary>
        /// <param name="BookingId"></param>>
        /// <remarks>
        /// Получение одного букинга по айди
        /// </remarks>
        /// <returns>GetBookingResponse</returns>
        [HttpGet]
        [Authorize]
        [Route(Routes.OneBookingRoute)]
        [ProducesResponseType(typeof(GetBookingResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBooking(Guid BookingId)
        {
            return Ok(await _bookingService.GetOneBooking(BookingId));
        }
    }
}

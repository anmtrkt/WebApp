using WebApp.DB;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace WebApp.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        public BookingService(Context context, UserManager<User> userManager) 
        {
            _context = context;
            _userManager = userManager; ;
        }

        public async Task<List<Booking>?> GetUserBookings(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await _context.Bookings.Where(b => b.User == user).ToListAsync();
        }
        public async Task<int> CreateBooking(Hotel hotel, Room room, DateTime DateFrom, DateTime DateTo)
        {
            var booking = new Booking
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                RoomId = room.Id,
                HotelId = hotel.Id,
                TotalDays = 1,
                TotalCost = 2,
                Price = 10,
            };
            _context.AddAsync(booking);
            _context.SaveChanges();
            return await _context.SaveChangesAsync(); ;
        }
    }
}

using WebApp.DB;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;

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
        public async Task<int> CreateBooking(int HotelId, int RoomId, User user, DateTime DateFrom, DateTime DateTo)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == RoomId);
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == HotelId);
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                DateFrom = DateFrom,
                DateTo = DateTo,
                RoomId = room.Id,//room.Id,
                TotalDays = (DateTo - DateFrom).Days,
                TotalCost = room.Price * (DateTo - DateFrom).Days,
                UserId = user.Id,
            };
            _context.AddAsync(booking);
            _context.SaveChanges();
            return await _context.SaveChangesAsync(); ;
        }
    }
}

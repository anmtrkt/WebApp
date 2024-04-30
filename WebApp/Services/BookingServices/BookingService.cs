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

       /* public async Task<List<Booking>?> GetUserBookings(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
      
            return await _context.Bookings.Where(b => b.User == user).ToListAsync();

        }*/
        public async Task<int> CreateBooking(/*Hotel hotel, Room room, User user,*/ DateTime DateFrom, DateTime DateTo)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                DateFrom = DateFrom,
                DateTo = DateTo,
                RoomId = 12,//room.Id,
                HotelId = 11, //hotel.Id,
                TotalDays = (DateTo - DateFrom).Days,
                TotalCost = /*room.Price*/ 5 * (DateTo - DateFrom).Days,
                UserId = Guid.NewGuid(),
            };
            _context.AddAsync(booking);
            _context.SaveChanges();
            return await _context.SaveChangesAsync(); ;
        }
    }
}

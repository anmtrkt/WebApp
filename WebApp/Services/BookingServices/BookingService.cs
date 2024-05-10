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
using Microsoft.AspNetCore.Http.HttpResults;

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

        public async Task<GetBookingResponse> GetUserBookings(string email)
        {


            var user = await _context.Users
                                    .FirstOrDefaultAsync(u => u.Email == email);
            var bookings = await _context.Bookings
                                    .Include(b => b.Rooms)
                                    .ThenInclude(r => r.Hotel)
                                    .Where(b => b.UserId == user.Id)
                                    .ToListAsync();
            
            return bookings.Select(b => new GetBookingResponse
            {
                TotalDays = b.TotalDays,
                TotalCost = b.TotalCost,
                DateFrom = b.DateFrom,
                DateTo = b.DateTo,
                Rooms = b.Rooms,
                HotelName = b.Hotel.Name,
                HotelLocation = b.Hotel.Location,
            });

        }
        public async Task<string> CreateBooking(int HotelId, int RoomId, User user, DateTime DateFrom, DateTime DateTo)
        {
            var room = await _context.Rooms
                                .FirstOrDefaultAsync(r => r.Id == RoomId);
            var BookedRooms = await _context.Bookings
                                        .CountAsync(r => r.RoomId == RoomId &&(r.DateFrom >= DateFrom && r.DateFrom <= DateTo) || (r.DateFrom <= DateFrom && r.DateTo > DateFrom));
            var RoomLeft = room.Quantity - BookedRooms;
            var hotel = await _context.Hotels
                                .FirstOrDefaultAsync(h => h.Id == HotelId);
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                DateFrom = DateFrom,
                DateTo = DateTo,
                RoomId = room.Id,
                TotalDays = (DateTo - DateFrom).Days,
                TotalCost = room.Price * (DateTo - DateFrom).Days,
                UserId = user.Id,
            };
            _context.AddAsync(booking);
            await _context.SaveChangesAsync();
            return "BOOKING CREATED"; 
        }
    }
}

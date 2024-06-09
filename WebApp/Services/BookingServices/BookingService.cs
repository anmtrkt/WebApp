using WebApp.DB;
using WebApp.Extensions;
using WebApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.X509Certificates;
using WebApp.Models.BookingsModel;
using Hangfire;

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
            _userManager = userManager; 
        }

        public async Task<List<GetBookingResponse?>> GetUserBookings(string email)
        {
            
            var user = await _context.Users
                                    .FirstOrDefaultAsync(u => u.Email == email);
            var booking = await _context.Bookings.Where(b => b.UserId == user.Id).Select(b => new
            {
                b.DateFrom,
                b.DateTo,
                b.TotalDays,
                b.TotalCost,
                b.Hotel,
                b.Rooms,
            }).ToListAsync();
            
            var Response = booking.Select(item => new GetBookingResponse
            {
                DateFrom = item.DateFrom,
                DateTo = item.DateTo,
                TotalDays = item.TotalDays,
                TotalCost = item.TotalCost,
                HotelLocation = item.Hotel.Location,
                HotelName = item.Hotel.Name,
                RoomName = item.Rooms.Name,
                Description = item.Rooms.Description,
                Services = item.Rooms.Services,
                Stars = item.Hotel.Stars,

            }).ToList();
            return Response;

        }
        public async Task<GetBookingResponse> CreateBooking(int RoomId, int HotelId, User user, DateOnly DateFrom, DateOnly DateTo)
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
                HotelId = hotel.Id,
                TotalDays = DateTo.DayNumber - DateFrom.DayNumber,
                TotalCost = room.Price * (DateTo.DayNumber - DateFrom.DayNumber),
                UserId = user.Id,
            };
            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();
            var Response =  new GetBookingResponse
            {
                DateFrom = booking.DateFrom,
                DateTo = booking.DateTo,
                TotalDays = booking.TotalDays,
                TotalCost = booking.TotalCost,
                HotelLocation = booking.Hotel.Location,
                HotelName = booking.Hotel.Name,
                RoomName = booking.Rooms.Name,
                Description = booking.Rooms.Description,
                Services = booking.Rooms.Services,
                Stars = booking.Hotel.Stars,
            };
            return Response; 
        }
        public async Task<bool> DeleteUserBooking(Guid BookingId)
        {
            var DeletedBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == BookingId);
            if (DeletedBooking!=null){
                _context.Bookings.Remove(DeletedBooking);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<object?> GetOneBooking(Guid BookingId)
        {
            var Booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == BookingId);
            if (Booking != null)
            {
                var Response = new GetBookingResponse
                {
                    DateFrom = Booking.DateFrom,
                    DateTo = Booking.DateTo,
                    TotalDays = Booking.TotalDays,
                    TotalCost = Booking.TotalCost,
                    HotelLocation = Booking.Hotel.Location,
                    HotelName = Booking.Hotel.Name,
                    RoomName = Booking.Rooms.Name,
                    Description = Booking.Rooms.Description,
                    Services = Booking.Rooms.Services,
                    Stars = Booking.Hotel.Stars,
                };
                return Response;
            }
            else
            {
                return null;
            }
        }
    }
}

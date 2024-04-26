using WebApp.DB;
using WebApp.Models;
namespace WebApp.Services.BookingServices

{
    public interface IBookingService
    {
        Task<List<Booking?>?> GetUserBookings(string email);
        Task<int> CreateBooking(Hotel hotel, Room room, DateTime DateFrom, DateTime DateTo);
        //bool DeleteBooking(Booking booking);
        
    }
}

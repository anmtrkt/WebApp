using WebApp.DB;
using WebApp.Models;
namespace WebApp.Services.BookingServices

{
    public interface IBookingService
    {
        Task<List<Booking?>?> GetUserBookings(string email);
        Task<int> CreateBooking(int HotelId, int RoomId,User user, DateTime DateFrom, DateTime DateTo);
        //bool DeleteBooking(Booking booking);
        
    }
}

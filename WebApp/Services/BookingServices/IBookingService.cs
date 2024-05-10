using WebApp.DB;
using WebApp.Models;
namespace WebApp.Services.BookingServices

{
    public interface IBookingService
    {
        Task<GetBookingResponse?> GetUserBookings(string email);
        Task<string> CreateBooking(int HotelId, int RoomId,User user, DateTime DateFrom, DateTime DateTo);
        //bool DeleteBooking(Booking booking);
        
    }
}

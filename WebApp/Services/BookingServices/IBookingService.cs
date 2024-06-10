using WebApp.DB;
using WebApp.Models.BookingsModel;
namespace WebApp.Services.BookingServices

{
    public interface IBookingService
    {
        Task<object?> GetOneBooking(Guid BokingId);
        Task<List<GetBookingResponse?>> GetUserBookings(string email);
        Task<bool> DeleteUserBooking(Guid BookingId);
        Task<GetBookingResponse?> CreateBooking(int HotelId, int RoomId,User user, DateOnly DateFrom, DateOnly DateTo);

        
    }
}

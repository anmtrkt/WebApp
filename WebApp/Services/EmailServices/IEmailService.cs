using WebApp.Models.BookingsModel;

namespace WebApp.Services.EmailServices
{
    public interface IEmailService
    {
        public Task<bool> RegisterMail(string email, string name);
        public Task<bool> BookingMail(string email, string name, GetBookingResponse response);
        public Task<bool> BookingReminderMail(string email, string name, string RoomName, float TotalCost, DateOnly DateFrom, DateOnly DateTo, string HotelName, string HotelLocation, int TotalDays);
        public Task<bool> Reminder(DateOnly DateFrom, string email, string name, GetBookingResponse response);
    }
}

using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security;
using WebApp.Extensions;
using WebApp.Models.BookingsModel;

namespace WebApp.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> RegisterMail(string email, string name)
        {
            string header = $"Поздравляю, {name}!";
            string body = $"Вы успешно зарегистрировались!!!";
            var sended = _configuration.Sending(email, header, body);
            if (!sended)
                return false;
            return true;
        }
        public async Task<bool> BookingMail(string email, string name, GetBookingResponse response)
        {
            string header = $"Резервирование";
            string body = $"{name}, вы зарезервировали комнату {response.RoomName} " +
                $"в {response.HotelName}, по адресу {response.HotelLocation} с {response.DateFrom} " +
                $"по {response.DateTo} на {response.TotalDays} дней, стоимостью {response.TotalCost}";
            var sended = _configuration.Sending(email, header, body);
            if (!sended)
                return false;
            return true;
        }
        public async Task<bool> BookingReminderMail(string email, string name, string RoomName, float TotalCost, DateOnly DateFrom, DateOnly DateTo, string HotelName, string HotelLocation, int TotalDays)
        {
            string header = $"Резервирование";
            string body = $"{name},напоминаем, что вы зарезервировали комнату {RoomName} " +
                $"в {HotelName}, по адресу {HotelLocation} с {DateFrom} " +
                $"по {DateTo} на {TotalDays} дней, стоимостью {TotalCost}." +
                $" До действия бронирования осталось {DateFrom.DayNumber - DateOnly.FromDateTime(DateTime.UtcNow).DayNumber} дней";
            var sended = _configuration.Sending(email, header, body);
            if (!sended)
                return false;
            return true;
        }
        public async Task<bool> Reminder(DateOnly DateFrom, string email, string name, GetBookingResponse response)
        {
            var Today = DateTime.UtcNow;
            var reservedDay = DateFrom.ToDateTime(new TimeOnly());
            var remainingDays = reservedDay - Today;

           
            BackgroundJob.Schedule(
() => BookingReminderMail(email, name, response.RoomName, response.TotalCost, response.DateFrom, response.DateTo, response.HotelName, response.HotelLocation, response.TotalDays), reservedDay.AddDays(-7));
            BackgroundJob.Schedule(
() => BookingReminderMail(email, name, response.RoomName, response.TotalCost, response.DateFrom, response.DateTo, response.HotelName, response.HotelLocation, response.TotalDays), reservedDay.AddDays(-3));
            BackgroundJob.Schedule(
() => BookingReminderMail(email, name, response.RoomName, response.TotalCost, response.DateFrom, response.DateTo, response.HotelName, response.HotelLocation, response.TotalDays), reservedDay.AddDays(-1));
            return true;

        }
    }
}

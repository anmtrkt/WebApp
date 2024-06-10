using MailKit;
using Microsoft.AspNetCore.Identity;
using WebApp.DB;
using WebApp.Services.AdminServices;
using WebApp.Services.BookingServices;
using WebApp.Services.HotelServices;
using WebApp.Services.Identity;
using WebApp.Services.RoomServices;
using WebApp.Services.UserServices;
using WebApp.Services.EmailServices;


namespace WebApp.RegistrationServices
{
    public static class ApiRegistrationExtensions
    {
        /// <summary>
        /// Регистрация сервисов приложения
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IHotelService, HotelService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddScoped<IEmailService, EmailService>();



            return services;
        }
      
    }
}

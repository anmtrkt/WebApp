using WebApp.DB;
using WebApp.Services.BookingServices;
using WebApp.Services.Identity;
using WebApp.Services.RoomServices;
using WebApp.Services.UserServices;

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

            return services;
        }
    }
}

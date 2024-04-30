using Microsoft.AspNetCore.Identity;

namespace WebApp.DB
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            //Bookings = [];
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
        public int Sex { get; set; }
        public DateTime? BirthDate { get; set; }

        //Поля для токена
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        //public virtual ICollection<Booking> Bookings { get; set; }
    }
}

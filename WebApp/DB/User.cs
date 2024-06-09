using Microsoft.AspNetCore.Identity;

namespace WebApp.DB
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
        }

        /// <summary>
        /// ��� ������������
        /// </summary>
        public string Name { get; set; }
        public int Sex { get; set; }
        public DateOnly BirthDate { get; set; }

        //���� ��� ������
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}

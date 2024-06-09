namespace WebApp.Models.UsersModel
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}

namespace WebApp.DB
{
    public class Hotel
    {
        public Hotel()
        {
            Rooms = [];
        }
        public Guid Id { get; set; } //primary key!! done
        public string? Name { get; set; }
        public string? Location { get; set; }

        public virtual ICollection<Room> Rooms {get; set; }
    }
}
namespace WebApp.DB
{
    public class Booking
    {
        public Booking()
        {
           Rooms = [];
        }
        public Guid Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
        public int RoomId { get; set; } // foreign Key?? . done
        public Guid UserId { get; set; } // foreign key too .done
        public virtual User User { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
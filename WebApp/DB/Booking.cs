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
        public float Price { get; set; }
        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; } // foreign Key?? . done
        public Guid UserId { get; set; } // foreign key too .done
        public virtual User User { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
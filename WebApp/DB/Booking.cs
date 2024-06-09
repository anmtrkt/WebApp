namespace WebApp.DB
{
    public class Booking
    {
        public Guid Id { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
        public int RoomId { get; set; } // foreign Key?? . done
        public Guid UserId { get; set; } // foreign key too .done
        public int HotelId { get; set; }
        public virtual User User { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual Room Rooms { get; set; }
    }
}
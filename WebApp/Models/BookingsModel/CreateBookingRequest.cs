namespace WebApp.Models
{
    public class CreateBookingRequest
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; } 
        public DateOnly DateFrom { get; set; } 
        public DateOnly DateTo { get; set;  }
    }
}

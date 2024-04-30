namespace WebApp.Models
{
    public class CreateBookingRequest
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; } 
        public DateTime DateFrom { get; set; } 
        public DateTime DateTo { get; set;  }
    }
}

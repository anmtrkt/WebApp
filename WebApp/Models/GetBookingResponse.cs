namespace WebApp.Models
{
    public class GetBookingResponse
    {
        public Guid Id { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }    
        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
    }
}

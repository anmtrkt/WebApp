using WebApp.DB;
namespace WebApp.Models
{
    public class GetBookingResponse
    {
        public GetBookingResponse()
        {
            Rooms = [];
        }
        public string HotelName { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public string Services { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
        public string HotelLocation { get; set; }
    }
}

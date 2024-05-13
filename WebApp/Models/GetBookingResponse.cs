using System.Text.Json;
using WebApp.DB;
namespace WebApp.Models
{
    public class GetBookingResponse
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
        public string HotelName { get; set; }
        public string HotelLocation { get; set; }

        public int Stars { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public JsonDocument Services { get; set; }

    }
}

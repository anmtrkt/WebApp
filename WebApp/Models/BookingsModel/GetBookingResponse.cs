using System.Text.Json;
using WebApp.DB;
using Newtonsoft;
using SixLabors.ImageSharp;
namespace WebApp.Models.BookingsModel
{
    public class GetBookingResponse
    {
        public Guid Id { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }

        public float TotalCost { get; set; }
        public int TotalDays { get; set; }
        public string HotelName { get; set; }
        public string HotelLocation { get; set; }

        public int Stars { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public JsonDocument Services { get; set; }
        //public Image Image { get; set; }

    }
}

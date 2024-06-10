using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CreateBookingRequest
    {
        [Required]
        public int HotelId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public DateOnly DateFrom { get; set; }
        [Required]
        public DateOnly DateTo { get; set;  }
    }
}

namespace WebApp.Models
{
    public class CreateRoomRequest
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string Services { get; set; }
        public int Quantity { get; set; }
        public int ImageId { get; set; }
    }
}

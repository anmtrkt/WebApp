using System.Text.Json;

namespace WebApp.DB
{
    public class Hotel
    {
        public Hotel()
        {
            Rooms = [];
        }
        public int Id { get; set; } //primary key!! done
        public string Name { get; set; }
        public string Location { get; set; }
        public int RoomsQuantity { get; set; }
        public int ImageId { get; set; }
        public JsonDocument Services { get; set; }
        public void Dispose() => Services?.Dispose();
        public int Stars { get; set; }

        public virtual ICollection<Room> Rooms {get; set; }
    }
}
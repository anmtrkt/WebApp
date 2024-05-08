using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebApp.DB
{
    public class Room
    {
        public int Id { get; set; } // Primary key.  done
        public int HotelId { get; set; } // Foreign Key. done
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public JsonDocument? Services { get; set; }
        public int Quantity { get; set; }
        public int ImageID { get; set; }
        public void Dispose() => Services?.Dispose();
        public virtual Hotel Hotel { get; set; }
    }
}
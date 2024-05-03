namespace WebApp.DB
{
    public class Room
    {
        public int Id { get; set; } // Primary key.  done
        public int HotelId { get; set; } // Foreign Key. done
        public string Name { get; set; }
        public int Price { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
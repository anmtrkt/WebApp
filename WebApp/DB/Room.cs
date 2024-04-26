namespace WebApp.DB
{
    public class Room
    {
        public Guid Id { get; set; } // Primary key.  done
        public Guid HotelId { get; set; } // Foreign Key. done
        public string Name { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
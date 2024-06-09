namespace WebApp.Models.HotelsModel
{
    public class CreateHotelRequest
    {//int Id, string Name, string Location, int RoomsQuantity, int ImageId, string Services, int Stars
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int RoomsQuantity { get; set; }
        public int ImageId { get; set; }
        public string Services { get; set; }
        public int Stars { get; set; }
    }
}

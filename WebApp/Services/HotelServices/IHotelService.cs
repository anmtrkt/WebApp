using WebApp.DB;

namespace WebApp.Services.HotelServices
{
    public interface IHotelService
    {
        Task<bool> DeleteHotel(int HotelId);
        Task<List<Hotel?>> GetAllHotels();
        Task<Hotel> CreateHotel(int Id,  string Name, string Location, int RoomsQuantity, int ImageId, string Services, int Stars);
    }
}

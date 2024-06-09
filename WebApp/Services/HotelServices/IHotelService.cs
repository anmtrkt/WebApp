using WebApp.DB;
using WebApp.Models.HotelsModel;

namespace WebApp.Services.HotelServices
{
    public interface IHotelService
    {
        Task<GetHotelResponse> GetOneHotel(int HotelId);
        Task<List<Room?>> GetHotelRooms(int HotelId);
        Task<bool> DeleteHotel(int HotelId);
        Task<List<GetHotelResponse?>> GetHotelsByLocationAndServices(string Location, string? UserServices, int? Stars);
        Task<List<GetHotelResponse?>> GetAllHotels();
        Task<GetHotelResponse> CreateHotel(int Id,  string Name, string Location, int RoomsQuantity, int ImageId, string Services, int Stars);
    }
}

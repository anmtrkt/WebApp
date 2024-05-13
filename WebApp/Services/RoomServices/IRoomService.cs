using System.Text.Json;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.Services.RoomServices
{
    public interface IRoomService
    {
        Task<List<Room?>> GetAllRooms();
        Task<List<Room?>> GetHotelRooms(int HotelId);
        Task<Room> CreateRoom(int Id, int HotelId, string Name, string? Description, int Price, string Services, int Quantity, int ImageId);
    }
}

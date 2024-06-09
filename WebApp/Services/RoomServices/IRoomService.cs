using System.Text.Json;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.Services.RoomServices
{
    public interface IRoomService
    {
        Task<List<Room?>> FindRoom(string Services, int MinPrice, int MaxPrice);
        Task <bool> DeleteRoom(int RoomId);
        Task<List<Room?>> GetAllRooms();
        Task<Room> JustRoom(int RoomId);
        Task<Room> CreateRoom(int Id, int HotelId, string Name, string? Description, int Price, string Services, int Quantity, int ImageId);
    }
}

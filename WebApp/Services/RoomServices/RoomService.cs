using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp.DB;

namespace WebApp.Services.RoomServices
{
    public class RoomService: IRoomService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        public RoomService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager; ;
        }
        public async Task<List<Room?>> GetAllRooms()
        {
            var RoomsList = await _context.Rooms.ToListAsync();
            return RoomsList;
        }
        public async Task<List<Room?>> GetHotelRooms(int HotelId)
        {
            var RoomsList = await _context.Rooms.Where(r => r.HotelId == HotelId).ToListAsync();
            return RoomsList;
        }
        public async Task<Room> CreateRoom(int Id, int HotelId, string Name, string? Description, int Price, string Services, int Quantity, int ImageId)
        {
            string[] ArrServices = Services.Split(',').Select(s => s.Trim()).ToArray();
            JsonDocument JSONServices = JsonDocument.Parse(JsonSerializer.Serialize(ArrServices));
            var ThisHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == HotelId);
            var NewRoom = new Room
            {
                Id = Id,
                HotelId = HotelId,
                Name = Name,
                Description = Description,
                Price = Price,
                Services = JSONServices,
                Quantity = Quantity,
                ImageID = ImageId,
                Hotel = ThisHotel,
            };
            await _context.AddAsync(NewRoom);
            await _context.SaveChangesAsync();
            return NewRoom;
        }
    }
}

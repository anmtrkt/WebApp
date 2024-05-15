using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp.DB;
using WebApp.Services.RoomServices;

namespace WebApp.Services.HotelServices
{
    public class HotelService : IHotelService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        public HotelService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager; ;
        }
        public async Task<List<Hotel?>> GetAllHotels()
        {
            var HotelsList = await _context.Hotels.ToListAsync();
            return HotelsList;
        }
        public async Task<Hotel> CreateHotel(int Id, string Name, string Location, int RoomsQuantity, int ImageId, string Services, int Stars)
        {
            string[] ArrServices = Services.Split(',').Select(s => s.Trim()).ToArray();
            JsonDocument JSONServices = JsonDocument.Parse(JsonSerializer.Serialize(ArrServices));
            var NewHotel = new Hotel
            {
                Id = Id,
                Name = Name,
                Location = Location,
                RoomsQuantity = RoomsQuantity,
                Services = JSONServices,
                ImageId = ImageId,
                Stars = Stars,
            };
            await _context.AddAsync(NewHotel);
            await _context.SaveChangesAsync();
            return NewHotel;
        }
        public async Task<bool> DeleteHotel(int HotelId)
        {
            var Hotel = await _context.Hotels.Where(r => r.Id == HotelId).FirstOrDefaultAsync();
            if (Hotel == null)
            {
                return false;
            }
            else
            {
                await _context.Rooms.Where(r => r.HotelId == Hotel.Id).ExecuteDeleteAsync();
                _context.Hotels.Remove(Hotel);
                await _context.SaveChangesAsync();
                return true;
            }

        }
    }
}

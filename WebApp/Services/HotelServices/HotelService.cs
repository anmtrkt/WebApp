using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text.Json;
using System.Xml.Linq;
using WebApp.DB;
using WebApp.Services.RoomServices;
using WebApp.Models.HotelsModel;

namespace WebApp.Services.HotelServices
{
    public class HotelService : IHotelService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _cacheManager;
        public HotelService(Context context, UserManager<User> userManager, IDistributedCache distributedCache)
        {
            _context = context;
            _userManager = userManager; 
            _cacheManager = distributedCache;
        }
        public async Task<List<GetHotelResponse?>> GetHotelsByLocationAndServices(string Location, string? UserServices, int? Stars)
        {
            List<Hotel>? HotelsList = null;
            string NormLocation = Location.ToLower().ToUpper();
            
            var cachedHotels = await _cacheManager.GetStringAsync(NormLocation);
            if (cachedHotels != null) HotelsList = JsonSerializer.Deserialize<List<Hotel>>(cachedHotels);
            if (HotelsList == null)
            {
                HotelsList = await _context.Hotels.Where(h => h.NormalizedLocation.Contains(NormLocation)).ToListAsync();
                if (HotelsList != null)
                {
                    Console.WriteLine($"ОТЕЛИ извлечен из базы данных");
                    cachedHotels = JsonSerializer.Serialize(HotelsList);
                    await _cacheManager.SetStringAsync(NormLocation, cachedHotels, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                    });
                }
            }
            else
            {
                Console.WriteLine($"ОТЕЛИ извлечен из кэша");
            }
            if (UserServices != null && Stars != null)
            {
                var ServicesList = HotelsList.Select(item => new
                {

                    Id = item.Id,
                    Stars = item.Stars,
                    Services = JsonSerializer.Deserialize<string[]>(item.Services)
                }).ToList();
                var RequiredHotels = ServicesList.Where(S => S.Services.Any(service => UserServices.Contains(service) && S.Stars == Stars)).ToList();
                HotelsList = await _context.Hotels.Where(h => RequiredHotels.Select(S => S.Id).Contains(h.Id)).ToListAsync();
            }
            else if (UserServices != null && Stars == null)
            {
                var ServicesList = HotelsList.Select(item => new
                {
                    Id = item.Id,
                    Services = JsonSerializer.Deserialize<string[]>(item.Services)
                }).ToList();
                var RequiredHotels = ServicesList.Where(S => S.Services.Any(service => UserServices.Contains(service))).ToList();
                HotelsList = await _context.Hotels.Where(h => RequiredHotels.Select(S => S.Id).Contains(h.Id)).ToListAsync();
            
            }
            else if (UserServices == null && Stars != null)
            {
                var ServicesList = HotelsList.Select(item => new
                {
                    Id = item.Id,
                    Stars = item.Stars
                }).ToList();
                var RequiredHotels = ServicesList.Where(S => S.Stars == Stars).ToList();
                HotelsList = await _context.Hotels.Where(h => RequiredHotels.Select(S => S.Id).Contains(h.Id)).ToListAsync();
            }
            var Response = HotelsList.Select(item => new GetHotelResponse
            {
                Id = item.Id,
                Name = item.Name,
                Location = item.Location,
                Services = item.Services,
                Stars = item.Stars,
                RoomsQuantity = item.RoomsQuantity,
                ImageId = item.ImageId
            }).ToList();
            return Response;
        }
        public async Task<List<GetHotelResponse?>> GetAllHotels()
        {
            var HotelsList = await _context.Hotels.ToListAsync();
            var Response = HotelsList.Select(item => new GetHotelResponse
            {
                Id = item.Id,
                Name = item.Name,
                Location = item.Location,
                Services = item.Services,
                Stars = item.Stars,
                RoomsQuantity = item.RoomsQuantity,
                ImageId = item.ImageId
            }).ToList();
            return Response;
        }
        public async Task<GetHotelResponse?> GetOneHotel(int HotelId)
        {
            var Hotel = await _context.Hotels.FirstOrDefaultAsync(H => H.Id == HotelId);
            string? filePath = Path.Combine("static", "images", $"{Hotel.ImageId}.webp");
            byte[] imageData = System.IO.File.ReadAllBytes(filePath);

            var image= new FileContentResult(imageData, "image/webp");

            return new GetHotelResponse
                {
                    Id = Hotel.Id,
                    RoomsQuantity = Hotel.RoomsQuantity,
                    Services = Hotel.Services,
                    Name = Hotel.Name,
                    Location = Hotel.Location,
                    Stars = Hotel.Stars,
                    ImageId = Hotel.ImageId,
                    Image = image
                };
            
        }
        public async Task<List<Room?>> GetHotelRooms(int HotelId)
        {
            var RoomsList = await _context.Rooms.Where(r => r.HotelId == HotelId).ToListAsync();
            return RoomsList;
        }
        public async Task<GetHotelResponse> CreateHotel(int Id, string Name, string Location, int RoomsQuantity, int ImageId, string Services, int Stars)
        {
            string[] ArrServices = Services.Split(',').Select(s => s.Trim()).ToArray();
            JsonDocument JSONServices = JsonDocument.Parse(JsonSerializer.Serialize(ArrServices));
            var NewHotel = new Hotel
            {
                Id = Id,
                Name = Name,
                NormalizedName = Name.Normalize(),
                Location = Location,
                NormalizedLocation = Location.Normalize(),
                RoomsQuantity = RoomsQuantity,
                Services = JSONServices,
                ImageId = ImageId,
                Stars = Stars,
            };
            await _context.AddAsync(NewHotel);
            await _context.SaveChangesAsync();
            return new GetHotelResponse
            {
                Id = Id,
                Name = Name,
                Location = Location,
                RoomsQuantity = RoomsQuantity,
                Services = JSONServices,
                ImageId = ImageId,
                Stars = Stars
            };
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

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WebApp.DB;
using WebApp.Models.RoomsModel;

namespace WebApp.Services.RoomServices
{
    public class RoomService : IRoomService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _cacheManager;
        public RoomService(Context context, UserManager<User> userManager, IDistributedCache distributedCache)
        {
            _context = context;
            _userManager = userManager;
            _cacheManager = distributedCache;
        }
        public async Task<List<Room?>> GetAllRooms()
        {
            var RoomsList = await _context.Rooms.ToListAsync();
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
        public async Task<bool> DeleteRoom(int RoomId)
        {
            var Room = await _context.Rooms.Where(r => r.Id == RoomId).FirstOrDefaultAsync();
            if (Room == null)
            {
                return false;
            }
            else
            {
                _context.Rooms.Remove(Room);
                await _context.SaveChangesAsync();
                return true;
            }

        }
        public async Task<List<Room?>> FindRoom(string UserServices, int MinPrice, int MaxPrice)
        {
            List<Room> RoomsList = null;
            var cachedRooms = await _cacheManager.GetStringAsync("AllRooms");

            if (cachedRooms != null) RoomsList = JsonSerializer.Deserialize<List<Room>>(cachedRooms);
            if (RoomsList == null)
            {
                RoomsList = await _context.Rooms.ToListAsync();
                cachedRooms = JsonSerializer.Serialize(RoomsList);
                await _cacheManager.SetStringAsync("AllRooms", cachedRooms, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
                var ServicesList = RoomsList.Select(item => new FindServices
                {
                    Id = item.Id,
                    Services = JsonSerializer.Deserialize<string[]>(item.Services)
                }).ToList();

                var RequiredRooms = ServicesList.Where(S => S.Services.Any(service => UserServices.Contains(service))).ToList();
                var RoomsWithServices = await _context.Rooms.Where(r => r.Price >= MinPrice && r.Price <= MaxPrice && RequiredRooms.Select(S => S.Id).Contains(r.Id)).ToListAsync();

                return RoomsWithServices;
            }
            else
            {
                var ServicesList = RoomsList.Select(item => new FindServices
                {
                    Id = item.Id,
                    Services = JsonSerializer.Deserialize<string[]>(item.Services)
                }).ToList();

                var RequiredRooms = ServicesList.Where(S => S.Services.Any(service => UserServices.Contains(service))).ToList();
                var RoomsWithServices = _context.Rooms.Where(r => r.Price >= MinPrice && r.Price <= MaxPrice && RequiredRooms.Select(S => S.Id).Contains(r.Id)).ToList();

                return RoomsWithServices;
            }
        }
                /*
            List<Room> RoomsList = null;
            var cachedRooms = await _cacheManager.GetStringAsync("AllRooms");
            if (cachedRooms!=null) RoomsList = JsonSerializer.Deserialize<List<Room>>(cachedRooms);
            if (RoomsList == null)
            {
                RoomsList = await _context.Rooms.ToListAsync();

                if (MinPrice != null && MaxPrice != null)
                {
                    var ServicesList = RoomsList.Select(item => new FindServices
                    {
                        Id = item.Id,
                        Services = JsonSerializer.Deserialize<string>(item.Services)
                    });
                    var RequiredRooms = 
                    return RoomsList;
                }
            }
            else
            {
                var RoomsList = await _context.Rooms.Where(r => r.Services.ToString().Contains(Services))
                    .ToListAsync();
                return RoomsList;
            }*/
        
        public async Task<Room> JustRoom(int RoomId)
        {
            var Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == RoomId);
            return Room;
        }
    }

}


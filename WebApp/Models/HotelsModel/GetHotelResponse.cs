using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Text.Json;

namespace WebApp.Models.HotelsModel
{
    public class GetHotelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int RoomsQuantity { get; set; }
        public int ImageId { get; set; }
        public JsonDocument Services { get; set; }
        public int Stars { get; set; }
        public IResult? Image { get; set; }

    }
}

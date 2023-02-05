using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SellAI.Models.DTOs
{
    public class BrandDTO
    {
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public bool isActive { get; set; } = false;
        public string app { get; set; } = "";
    }
}

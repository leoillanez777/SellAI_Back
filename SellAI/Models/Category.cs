using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SellAI.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string name { get; set; } = null!;

        [BsonElement("description")]
        [JsonProperty("description")]
        public string description { get; set; } = null!;

        [BsonElement("isActive")]
        [JsonProperty("activo")]
        public bool isActive { get; set; }

        [BsonElement("app")]
        [JsonProperty("app")]
        public string app { get; set; } = null!;
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SellAI.Models.DTOs {
  public class CategoryDTO {
    [JsonProperty("id")]
    public string Id { get; set; } = null!;
    [JsonProperty("nombre")]
    public string Nombre { get; set; } = null!;
    [JsonProperty("descripcion")]
    public string Descripcion { get; set; } = null!;
    [JsonProperty("sinonimos")]
    public List<string> Sinonimos { get; set; } = null!;
    [JsonProperty("activo")]
    public bool Activo { get; set; } = false;
  }
}

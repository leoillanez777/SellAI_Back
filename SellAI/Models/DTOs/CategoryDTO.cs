using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SellAI.Models.DTOs {
  public class CategoryDTO {
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public List<string> Sinonimos { get; set; } = null!;
    public bool Activo { get; set; } = false;
  }
}

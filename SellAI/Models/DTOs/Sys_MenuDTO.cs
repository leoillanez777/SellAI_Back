using System;
using Newtonsoft.Json;
using SellAI.Models.Objects;

namespace SellAI.Models.DTOs;

public class Sys_MenuDTO {

  [JsonProperty("id")]
  public string? Id { get; set; }

  [JsonProperty("id_intent")]
  public string IntentID { get; set; } = null!;

  [JsonProperty("name")]
  public string Nombre { get; set; } = null!;

  [JsonProperty("display")]
  public string Display { get; set; } = null!;

  [JsonProperty("action")]
  public string Accion { get; set; } = null!;

  [JsonProperty("collection")]
  public string Collection { get; set; } = null!;

  [JsonProperty("entities")]
  public List<Entity>? Entities { get; set; }

  [JsonProperty("pdf")]
  public bool? Pdf { get; set; }

  [JsonProperty("roles")]
  public List<string>? Roles { get; set; }

  [JsonProperty("message")]
  public string? Mensaje { get; set; }

  [JsonProperty("type")]
  public string Tipo { get; set; } = null!;
}

using System;
namespace SellAI.Models.DTOs.Object
{
  public class TableDto
  {
    public List<ColumnDto> columns { get; set; } = new();
    public List<Dictionary<string, string>> rows { get; set; } = new();
    public string style { get; set; } = string.Empty;
  }
}


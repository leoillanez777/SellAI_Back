﻿using System;
using SellAI.Models.AI;

namespace SellAI.Models.DTOs
{
  public class Sys_ContextDTO
  {
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public bool Created { get; set; }
  }
}


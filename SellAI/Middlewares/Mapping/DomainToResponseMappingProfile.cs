using System;
using AutoMapper;
using SellAI.Models;
using SellAI.Models.AI.Objects;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Middlewares.Mapping
{
  public class DomainToResponseMappingProfile : Profile {
    public DomainToResponseMappingProfile()
    {
      CreateMap<Entity, Entities>()
        .ReverseMap();
      CreateMap<Category, CategoryDTO>()
        .ReverseMap();
      CreateMap<Brand, BrandDTO>()
        .ReverseMap();
    }
  }
}


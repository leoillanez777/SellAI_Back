using System;
using AutoMapper;
using SellAI.Models;
using SellAI.Models.AI.Objects;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Mapping
{
  public class DomainToResponseMappingProfile : Profile {
    public DomainToResponseMappingProfile()
    {
      CreateMap<Entity, Entities>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RolID))
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Rol))
        .ReverseMap();
      CreateMap<Category, CategoryDTO>()
        .ReverseMap();
      CreateMap<Brand, BrandDTO>()
        .ReverseMap();
    }
  }
}


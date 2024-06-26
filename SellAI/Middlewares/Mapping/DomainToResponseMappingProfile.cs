﻿using System;
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
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
        .ReverseMap();
      CreateMap<Category, CategoryDTO>()
        .ReverseMap();
      CreateMap<Brand, BrandDTO>()
        .ReverseMap();
      CreateMap<Sys_Menu, Sys_MenuDTO>()
        .ReverseMap();
    }
  }
}


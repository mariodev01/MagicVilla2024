﻿using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API
{
    public class MappingConfig:Profile
    {

        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa,VillCreateaDto>().ReverseMap();
            CreateMap<Villa,VillaUpdateDto>().ReverseMap();




            CreateMap<NumeroVilla,NumeroVillaDto>().ReverseMap();
            CreateMap<NumeroVilla,NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVilla,NumeroVillaUpdateDto>().ReverseMap();
        }
    }
}

﻿using AutoMapper;
using LexiconLMS.Domain.Entities;
using LexiconLMS.Shared.Dtos;
using Microsoft.AspNetCore.Identity;

namespace LexiconLMS.Server.AutoMapperConfig
{
    public class LmsMappings : Profile
    {
        public LmsMappings() 
        { 
            CreateMap<Course, CourseDto>().ReverseMap();
            //CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap();
            CreateMap<Activity, ActivityDto>().ReverseMap();
            CreateMap<ActivityType, ActivityTypeDto>().ReverseMap();
            CreateMap<IdentityUserRole<string>, RoleDto>().ReverseMap();

            //CreateMap<ApplicationUser, UserDto>()
            //.ForMember(dto => dto.Role, opt => opt.MapFrom(x => x.Id.Select(y => y.RoleId).ToList()));

            //CreateMap<ApplicationUser, UserDto>()
            //    .ForMember(dto => dto.Role, opt => opt.MapFrom(x => x..Select(y => y.Providers).ToList()));

            //CreateMap<ApplicationUser, UserDto>().ForMember(dto => dto.Role, opt => opt.MapFrom(src => src.Role.Select(r => r.Role.Name).ToList()));

            
        }
    }
}

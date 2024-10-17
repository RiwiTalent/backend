using AutoMapper;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coder, CoderDto>();
        }
    }
}
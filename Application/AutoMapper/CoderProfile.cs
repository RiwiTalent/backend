using AutoMapper;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Application.AutoMapper
{
    public class CoderProfile : Profile
    {
        public CoderProfile ()
        {
            CreateMap<CoderDto, Coder>()
                .ForMember(dest => dest.Age, opt => 
                {
                    // opt.Condition(src => src.AgeUser > 0);
                    opt.PreCondition(src => src.Age > 0);
                    opt.MapFrom(src => src.Age);
                })
                .ForAllMembers ( opt => 
                {
                    opt.Condition((src, dest, sourceMember) => sourceMember != null);
                });

            CreateMap<Coder, CoderDto>();
        }
    }
}
using AutoMapper;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Application.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ForAllMembers ( opt => 
                {
                    opt.Condition((src, dest, sourceMember) => sourceMember != null);
                });
            CreateMap<User, UserDto>();
        }
    }
}
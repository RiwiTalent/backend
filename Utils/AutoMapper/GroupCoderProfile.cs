using AutoMapper;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models;

namespace RiwiTalent.Utils.AutoMapper
{
    public class GroupCoderProfile : Profile
    {
        public GroupCoderProfile() 
        {
           CreateMap<GroupCoderDto, Group>()
                .ForAllMembers(opt => 
                {
                    opt.AllowNull();
                    opt.Condition((src, dest, sourceMember) => sourceMember != null);
                });
            
            CreateMap<Group, GroupCoderDto>();
                                        /* .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); */ 
        } 
    }
}
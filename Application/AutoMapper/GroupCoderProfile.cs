using AutoMapper;
using RiwiTalent.Application.DTOs;
using System.Text.RegularExpressions;

namespace RiwiTalent.Application.AutoMapper
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
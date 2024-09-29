using AutoMapper;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models;

namespace RiwiTalent.Utils.AutoMapper
{
    public class GroupCoderProfile : Profile
    {
        public GroupCoderProfile() 
        {
           CreateMap<GroupCoderDto, GroupCoder>()
                .ForAllMembers(opt => 
                {
                    opt.AllowNull();
                    opt.Condition((src, dest, sourceMember) => sourceMember != null);
                });
            
            CreateMap<GroupCoder, GroupCoderDto>();
                                        /* .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); */ 
        } 
    }
}
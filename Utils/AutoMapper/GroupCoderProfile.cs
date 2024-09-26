using AutoMapper;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models;

namespace RiwiTalent.Utils.AutoMapper
{
    public class GroupCoderProfile : Profile
    {
        public GroupCoderProfile() 
        {
           CreateMap<GroupCoderDto, GruopCoder>()
                .ForAllMembers(opt => 
                {
                    opt.AllowNull();
                    opt.Condition((src, dest, sourceMember) => sourceMember != null);
                });
            
            CreateMap<GruopCoder, GroupCoderDto>();
                                        /* .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); */ 
        } 
    }
}
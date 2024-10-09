using AutoMapper;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Utils.AutoMapper
{
    public class TermAndConditionProfile : Profile
    {
        public TermAndConditionProfile()
        {
            CreateMap<TermAndCondition, TermAndConditionDto>()
                .ReverseMap(); 
        }
    }
}

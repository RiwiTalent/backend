using AutoMapper;
using MongoDB.Bson;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Application.AutoMapper
{
    public class TermAndConditionProfile : Profile
    {
        public TermAndConditionProfile()
        {
            CreateMap<TermAndConditionDto, TermAndCondition>()
                .ReverseMap(); 
        }
    }
}

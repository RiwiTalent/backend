using AutoMapper;
using MongoDB.Bson;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Application.AutoMapper
{
    public class GroupCoderProfile : Profile
    {
        public GroupCoderProfile()
        {
            // Mapeo de GroupCoderDto a Group
            CreateMap<GroupCoderDto, Group>()
                .ForMember(dest => 
                            dest.Id
                            , opt => opt.MapFrom(src => string.IsNullOrEmpty(
                            src.Id
                            ) ? ObjectId.GenerateNewId() : new ObjectId(
                            src.Id
                            )))
                .ForAllMembers(opt =>
                {
                    opt.Condition((src, dest, sourceMember) => sourceMember != null);
                });

            // Mapeo de Group a GroupCoderDto
            CreateMap<Group, GroupCoderDto>()
                .ForMember(dest => 
dest.Id
, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
} 

using backend.Models.Dtos;
using MongoDB.Bson;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Repository;

namespace RiwiTalent.Services.Interface
{
    public interface IGroupCoderRepository 
    {
        Task<IEnumerable<GroupCoderDto>> GetGroupCoders();
        Task<IEnumerable<Group>> GetGroupsInactive();
        Task<IEnumerable<Group>> GetGroupsActive();
        void Add(GroupDto groupDto);
        Task RegenerateToken(NewKeyDto newKeyDto);
        Task<KeyDto> SendToken(KeyDto keyDto);
        Task<GroupDetailsDto> GetGroupInfoById(string groupId);
        Task Update(GroupCoderDto groupCoderDto);
        // Task DeleteCoderGroup(string id);
        Task<bool> GroupExistByName(string name);
        Task DeleteGroup(string groupId);
    }
}
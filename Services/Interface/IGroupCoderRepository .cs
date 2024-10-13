using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Services.Interface
{
    public interface IGroupCoderRepository 
    {
        Task<IEnumerable<GroupCoderDto>> GetGroupCoders();
        Task<IEnumerable<Group>> GetGroupsInactive();
        Task<IEnumerable<Group>> GetGroupsActive();
        Task<Group> GetGroupByName(string name);
        void Add(GroupDto groupDto);
        Task RegenerateToken(NewKeyDto newKeyDto);
        Task<KeyDto> SendToken(KeyDto keyDto);
        Task<GroupDetailsDto> GetGroupInfoById(string groupId);
        Task Update(GroupCoderDto groupCoderDto);
        // Task DeleteCoderGroup(string id);
        Task DeleteGroup(string groupId);
    }
}
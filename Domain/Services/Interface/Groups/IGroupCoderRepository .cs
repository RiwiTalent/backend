using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;


namespace RiwiTalent.Domain.Services.Groups
{
    public interface IGroupCoderRepository 
    {
        Task<IEnumerable<GroupCoderDto>> GetGroupCoders();
        Task<IEnumerable<Group>> GetGroupsInactive();
        Task<IEnumerable<Group>> GetGroupsActive();
        Task<Group> GetGroupByName(string name);
        Task Add(GroupDto groupDto);
        Task RegenerateToken(NewKeyDto newKeyDto);
        Task<KeyDto> SendToken(KeyDto keyDto);
        Task<GroupDetailsDto> GetGroupInfoById(string groupId);
        Task Update(GroupCoderDto groupCoderDto);
        Task ReactiveGroup(string id);
        Task DeleteGroup(string groupId);
    }
}
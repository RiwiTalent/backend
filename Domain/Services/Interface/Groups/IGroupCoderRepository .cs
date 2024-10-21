using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;


namespace RiwiTalent.Domain.Services.Groups
{
    public interface IGroupCoderRepository 
    {
        Task<IEnumerable<GroupCoderDto>> GetGroupCoders();
        Task<IEnumerable<Group>> GetGroupsInactivo();
        Task<IEnumerable<Group>> GetGroupsActivo();
        Task<Group> GetGroupByName(string name);
        Task Add(GroupDto groupDto);
        Task UpdateGroupPhoto(string groupId, string photoUrl);
        Task RegenerateToken(string id);
        Task<KeyDto> SendToken(KeyDto keyDto);
        Task<GroupDetailsDto> GetGroupInfoById(string groupId);
        Task Update(GroupCoderDto groupCoderDto);
        Task ReactiveGroup(string id);
        Task DeleteGroup(string groupId);
    }
}
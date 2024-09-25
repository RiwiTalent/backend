using MongoDB.Bson;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Repository;

namespace RiwiTalent.Services.Interface
{
    public interface IGroupCoderRepository 
    {
        Task<IEnumerable<GroupCoderDto>> GetGroupCoders();
        (ObjectId, Guid) Add(GroupDto groupDto);
        Task<KeyDto> SendToken(GruopCoder groupCoder, string key);
        Task<GroupInfoDto> GetGroupInfoById(string groupId);
        Task Update(GroupCoderDto groupCoderDto);
        // Task DeleteCoderGroup(string id);
        Task<bool> GroupExistByName(string name);
    }
}
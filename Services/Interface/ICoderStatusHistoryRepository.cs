using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;

namespace RiwiTalent.Services.Interface
{
    public interface ICoderStatusHistoryRepository
    {
        Task<IEnumerable<CoderStatusHistory>> GetCodersHistoryStatus();
        Task<CoderHistoryDto> GetCoderHistoryById(string coderId);
        Task<CoderHistoryDto> GetGroupHistoryById(string groupId);
        Task<IEnumerable<CoderStatusHistory>> GetCompanyCoders(string id, Status status);
        // Task<Pagination<Coder>> GetCodersPagination(int page, int cantRegisters);
        // void Add(CoderStatusHistory coder);
        Task AddCodersGrouped(CoderGroupDto coderGroup);
        Task AddCodersSelected(CoderGroupDto coderGroup);
        Task DeleteCoderGroup(string id);
    }
}

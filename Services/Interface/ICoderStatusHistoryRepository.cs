using MongoDB.Bson;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;

namespace RiwiTalent.Services.Interface
{
    public interface ICoderStatusHistoryRepository
    {
        Task<IEnumerable<CoderStatusHistory>> GetCodersHistoryStatus();
        Task<IEnumerable<CoderStatusHistory>> GetCompanyCoders(string id, Status status);
        // Task<Pagination<Coder>> GetCodersPagination(int page, int cantRegisters);
        // void Add(CoderStatusHistory coder);
        void AddCodersGrouped(CoderGroupDto coderGroup);
        Task AddCodersSelected(CoderGroupDto coderGroup);
    }
}

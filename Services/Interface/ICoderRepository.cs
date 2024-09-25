using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Services.Interface
{
    public interface ICoderRepository
    {

        Task<IEnumerable<Coder>> GetCoders();
        Task<Pagination<Coder>> GetCodersPagination(int page, int cantRegisters);
        Task<IEnumerable<Coder>> GetCodersByGroup(string name);
        void Add(CoderDto coderDto);
        Task Update(CoderDto coderDto);
        Task UpdateCodersGroup(CoderGroupDto groupCoder);
        Task UpdateCodersSelected(CoderGroupDto groupCoder);
        Task<Coder> GetCoderId(string id);
        Task<Coder> GetCoderName(string name);
        void Delete(string id);    
        Task DeleteCoderGroup(string id);
        void ReactivateCoder(string id); 
        Task<List<Coder>> GetCodersBySkill(List<string> skill);
        Task<List<Coder>> GetCodersBylanguage(string level);
    }
}

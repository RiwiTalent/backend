using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Services.Interface
{
    public interface ICoderRepository
    {

        Task<Pagination<Coder>> GetCodersPagination(int page, int cantRegisters);
        Task<IEnumerable<Coder>> GetCodersByGroup(string name);
        void Add(Coder coder);
        Task Update(CoderDto coderDto);
        Task UpdateCodersGroup(CoderGroupDto gruopCoder);
        Task UpdateCodersSelected(CoderGroupDto gruopCoder);
        Task<Coder> GetCoderId(string id);
        Task<Coder> GetCoderName(string name);
        void Delete(string id);    
        void ReactivateCoder(string id);
        Task<List<Coder>> GetCodersByStack(List<string> stack);
        Task<List<Coder>> GetCodersBylanguage(string level);
    }
}

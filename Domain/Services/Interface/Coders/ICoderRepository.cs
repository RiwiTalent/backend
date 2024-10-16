using RiwiTalent.Domain.Entities;
using RiwiTalent.Application.DTOs;

namespace RiwiTalent.Domain.Services.Interface.Coders
{
    public interface ICoderRepository
    {

        Task<IEnumerable<Coder>> GetCoders();
        Task<Pagination<Coder>> GetCodersPagination(int page, int cantRegisters);
        Task Add(CoderDto coderDto);
        Task Update(Coder coder);
        Task UpdateCodersGroup(CoderGroupDto groupCoder);
        Task UpdateCodersSelected(CoderGroupDto groupCoder);
        Task<Coder> GetCoderId(string id);
        Task<Coder> GetCoderName(string name);
        Task<Coder> FindCoderById(string coderId);
        Task UpdateCoderPhoto(string coderId, string photoUrl); //cargar photo
        Task UpdateCoderCv(string coderId, string pdf); // cargar pdf
        Task Delete(string id);
        Task DeleteCoderGroup(string id);
        Task ReactivateCoder(string id); 
        Task<List<Coder>> GetCodersBySkill(List<string> skill);
        Task<List<Coder>> GetCodersBylanguage(string level);
    }
}

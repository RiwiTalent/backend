using System.Threading.Tasks;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Services.Interface
{
    public interface ITermAndConditionRepository
    {
        Task<List<TermAndCondition>> GetAllTermsAsync(); // Nuevo método para listar todos los términos
        Task<TermAndCondition?> GetTermsByEmailAsync(string email);
        Task Add();
        Task UpdateTermsAsync(TermAndConditionDto updatedTerms);
    }
}

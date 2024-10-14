using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Domain.Services.Interface.Terms
{
    public interface ITermAndConditionRepository
    {
        Task<List<TermAndCondition>> GetAllTermsAsync();
        Task<TermAndCondition?> GetTermsByEmailAsync(string email);
        Task Add(TermAndConditionDto termAndConditionDto);
        Task UpdateTermsAsync(TermAndConditionDto updatedTerms);
    }
}

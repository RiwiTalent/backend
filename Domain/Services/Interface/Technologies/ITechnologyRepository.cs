using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Domain.Services.Interface.Technologies
{
    public interface ITechnologyRepository
    {
        Task<IEnumerable<Technology>> GetTechnologies();
        Task Add(Technology technology);
        Task Update(Technology technology);
    }
}
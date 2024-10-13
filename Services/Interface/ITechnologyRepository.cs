using RiwiTalent.Models;

namespace RiwiTalent.Services.Interface
{
    public interface ITechnologyRepository
    {
        Task<IEnumerable<Technology>> GetTechnologies();
        void Add(Technology technology);
        Task Update(Technology technology);
    }
}
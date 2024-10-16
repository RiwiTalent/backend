using MongoDB.Driver;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Technologies;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Services.Repository
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private readonly IMongoCollection<Technology> _mongoCollection;
        public TechnologyRepository(MongoDbContext context)
        {
            _mongoCollection = context.Technologies;
        }
        public async Task Add(Technology technology)
        {
            await _mongoCollection.InsertOneAsync(technology);
        }
        public async Task<IEnumerable<Technology>> GetTechnologies()
        {
            var techs = await _mongoCollection.Find(_ => true)
                                                .ToListAsync();

            return techs;
        }

        public async Task Update(Technology technology)
        {
            var filter = Builders<Technology>.Filter.Eq(t => t.Id, technology.Id);
            var tech = _mongoCollection.Find(filter).FirstOrDefaultAsync();

            if(tech == null)
                throw new StatusError.ObjectIdNotFound("The document technology not found");

            var update = Builders<Technology>.Update.Set(t => t.Name, technology.Name);
            
            await _mongoCollection.UpdateOneAsync(filter, update);
        }
    }
}
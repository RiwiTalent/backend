using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.Services.Repository
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private readonly IMongoCollection<Technology> _mongoCollection;
        public TechnologyRepository(MongoDbContext context)
        {
            _mongoCollection = context.Technologies;
        }
        public void Add(Technology technology)
        {
            _mongoCollection.InsertOne(technology);
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
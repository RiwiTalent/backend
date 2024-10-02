using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var tech = await _mongoCollection.Find(t => t.Id == technology.Id).FirstOrDefaultAsync();

            if(tech == null)
                throw new StatusError.ObjectIdNotFound("The document Technology not found");
            
            var builder = Builders<Technology>.Filter.Eq(t => t.Id, technology.Id);
            var update = Builders<Technology>.Update.Set(t => t.Name, technology.Name);


            await _mongoCollection.UpdateOneAsync(builder, update);
        }
    }
}
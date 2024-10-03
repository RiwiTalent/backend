using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
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

        public async Task Update(string technologyId, int index, string newTecnology)
        {
            var filter = Builders<Technology>.Filter.Eq(t => t.Id, technologyId);
            var searchTech = _mongoCollection.Find(filter).FirstOrDefaultAsync();

            if(searchTech == null)
                throw new StatusError.ObjectIdNotFound("The document Technology not found");


            var update = Builders<Technology>.Update.Set($"Language_Programming.{index}", newTecnology);

            var result = await _mongoCollection.UpdateOneAsync(filter, update);

            if(result.ModifiedCount == 0)   
            {
                throw new StatusError.ObjectIdNotFound("The document Technology not found or no modification was made");
            }
            

        }
    }
}
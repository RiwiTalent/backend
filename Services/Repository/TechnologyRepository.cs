using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;

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
    }
}
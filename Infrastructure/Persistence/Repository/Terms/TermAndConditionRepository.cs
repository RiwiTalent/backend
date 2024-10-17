using MongoDB.Driver;
using AutoMapper;
using RiwiTalent.Domain.Services.Interface.Terms;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Application.DTOs;
using MongoDB.Bson;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Services.Repository
{
    public class TermAndConditionRepository : ITermAndConditionRepository
    {
       private readonly IMongoCollection<TermAndCondition> _mongoCollection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper; 

        public TermAndConditionRepository(MongoDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mongoCollection = context.TermsAndConditions;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper; 
        }

        public async Task<List<TermAndCondition>> GetAllTermsAsync() 
        {
            return await _mongoCollection.Find(tc => true).ToListAsync();
        }

        public async Task Add(TermAndConditionDto termAndConditionDto)
        {
            var newTerms = _mapper.Map<TermAndCondition>(termAndConditionDto); 
            await _mongoCollection.InsertOneAsync(newTerms); 
        }


        public async Task<TermAndCondition?> GetTermsByEmailAsync(string email)
        {
            return await _mongoCollection
                .Find(tc => tc.AcceptedEmail == email && tc.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateTermsAsync(TermAndConditionDto updatedTermsDto) 
        {
            var existTerms = await _mongoCollection.Find(tc => tc.Id == updatedTermsDto.Id).FirstOrDefaultAsync();

            if (existTerms == null)
            {
                throw new StatusError.ObjectIdNotFound("Terms not found");
            }

            // It is used AutoMapper to the information of Dto
            var termsToUpdate = _mapper.Map(updatedTermsDto, existTerms);
            var filter = Builders<TermAndCondition>.Filter.Eq(tc => tc.Id, termsToUpdate.Id);

            await _mongoCollection.ReplaceOneAsync(filter, termsToUpdate);
        }
    }
}

using System.Security.Claims;
using System.Threading.Tasks;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using AutoMapper;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;

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

        public async Task Add()
        {
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            var newObject = new TermAndCondition
            {
                Content = "Utils/Resources/TermsAndConditions.pdf",
                Clicked_Date = DateTime.UtcNow, 
                IsActive = false,
                Accepted = false,
                Version = 1,
                GroupId = null,
                AcceptedEmail = userEmail,
                CreatorEmail = userEmail
            };

            await _mongoCollection.InsertOneAsync(newObject);
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

            if (existTerms is null)
            {
                throw new Exception("Terms not found.");
            }

            // Usar AutoMapper para mapear desde el DTO a la entidad existente
            var termsToUpdate = _mapper.Map(updatedTermsDto, existTerms);
            var filter = Builders<TermAndCondition>.Filter.Eq(tc => tc.Id, termsToUpdate.Id);

            await _mongoCollection.ReplaceOneAsync(filter, termsToUpdate);
        }
    }
}

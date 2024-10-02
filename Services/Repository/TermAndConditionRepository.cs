using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.Services.Repository
{
    public class TermAndConditionRepository : ITermAndConditionRepository
    {
        private readonly IMongoCollection<TermAndCondition> _mongoCollection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TermAndConditionRepository(MongoDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mongoCollection = context.TermsAndConditions;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Add()
        {
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            TermAndCondition termAndCondition = new TermAndCondition();

            var newObject = new TermAndCondition
            {
                Content = "Utils/Resources/TermsAndConditions.pdf",
                Clicked_Date = termAndCondition.Clicked_Date,
                IsActive = true,
                Accepted = true,
                Version = 1,
                GroupId = null,
                AcceptedEmail = null,
                CreatorEmail = userEmail
            };

            await _mongoCollection.InsertOneAsync(newObject);

        }
    }
}
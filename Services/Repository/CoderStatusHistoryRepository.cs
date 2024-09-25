using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.Services.Repository
{
    public class CoderStatusHistoryRepository : ICoderStatusHistoryRepository
    {
        private readonly IMongoCollection<CoderStatusHistory> _mongoCollection;
        private readonly IMapper _mapper;

        private string Error = "The group not found";
        public CoderStatusHistoryRepository(MongoDbContext context, IMapper mapper)
        {
            _mongoCollection = context.CoderStatusHistories;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CoderStatusHistory>> GetCodersStatus()
        {
          List<CoderStatusHistory> coders = await _mongoCollection.Find(_ => true)
            .ToListAsync();

          return coders;
        }

        public async Task<IEnumerable<CoderStatusHistory>> GetCompanyGroupedCoders(string companyId)
        {
          var builder = Builders<CoderStatusHistory>.Filter;

          var query = _mongoCollection.Aggregate()
            .SortByDescending(p => p.Date)
            .Group(p => p.IdCoder, g => new
            { 
              Data = g.First()
            })
            .Match(new BsonDocument
            {
              { $"Data.{nameof(CoderStatusHistory.IdGroup)}", companyId },
              { $"Data.{nameof(CoderStatusHistory.Status)}", Status.Grouped.ToString() }
            })
            .ToList();
          
          IEnumerable<CoderStatusHistory> group = query.Select(r => r.Data);
          
          return group;
        }

        public void AddCodersGrouped(CoderGroupDto coderGroup)
        {
          AddCodersProccess(coderGroup, Status.Grouped);
        }

        public async Task AddCodersSelected(CoderGroupDto coderGroup)
        {
          AddCodersProccess(coderGroup, Status.Selected);

          IEnumerable<CoderStatusHistory> coders = await GetCompanyGroupedCoders(coderGroup.GruopId);

          foreach(CoderStatusHistory coderStatusHistory in coders)
          {
            coderStatusHistory.Status = Status.Active.ToString();
            Add(coderStatusHistory);
          }
        }

        private void AddCodersProccess(CoderGroupDto coderGroup, Status status)
        {
          List<string> coderIdList = coderGroup.CoderList;

          foreach(string coderId in coderIdList)
          {
            CoderStatusHistory coderStatusHistory = new CoderStatusHistory()
            {
              IdCoder = coderId,
              IdGroup = coderGroup.GruopId,
              Status = status.ToString()
            };

            Add(coderStatusHistory);
          }

        }

        private void Add(CoderStatusHistory coderStatusHistory)
        {
          coderStatusHistory.Id = null;
          coderStatusHistory.Date = DateTime.Now;
          _mongoCollection.InsertOne(coderStatusHistory);
        }

        public async Task<List<CoderStatusHistory>> GetCodersStatusById(string coderId)
        {
            try
            {
              var coders = Builders<CoderStatusHistory>.Filter.Eq(c => c.IdCoder, coderId);
              return await _mongoCollection.Find(coders).ToListAsync();
            }
            catch (Exception ex)
            {
              throw new ApplicationException("No existen coders por ese id.",ex);
            }
        }
    }
}
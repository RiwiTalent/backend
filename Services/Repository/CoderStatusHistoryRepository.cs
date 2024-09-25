using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Filters;

namespace RiwiTalent.Services.Repository
{
  public class CoderStatusHistoryRepository : ICoderStatusHistoryRepository
  {
    private readonly IMongoCollection<CoderStatusHistory> _mongoCollection;
    private readonly IMapper _mapper;
    private readonly ICoderRepository _coderRepository;

    public CoderStatusHistoryRepository(MongoDbContext context, IMapper mapper, ICoderRepository coderRepository)
    {
      _mongoCollection = context.CoderStatusHistories;
      _mapper = mapper;
      _coderRepository = coderRepository;
    }

    public async Task<IEnumerable<CoderStatusHistory>> GetCoderHistoryById(string coderId)
    {
      List<CoderStatusHistory> coderHistory = await _mongoCollection.Find(x => x.IdCoder == coderId)
        .ToListAsync();

      return coderHistory;
    }

    public async Task<IEnumerable<CoderStatusHistory>> GetCodersHistoryStatus()
    {
      List<CoderStatusHistory> coders = await _mongoCollection.Find(_ => true)
        .ToListAsync();

      return coders;
    }

    public async Task<IEnumerable<CoderStatusHistory>> GetCompanyCoders(string companyId, Status status)
    {
      // Traemos ambos estados, agrupados y seleccionados
      BsonDocument filter;

      if(status == Status.Active)
      {
        filter = MongoFilter.FilterByCompany(companyId);
        return GetUniqueLastRegisters(filter);
      }

      filter = MongoFilter.FilterByCompany(companyId, status);
      return GetUniqueLastRegisters(filter);
    }

    public void AddCodersGrouped(CoderGroupDto coderGroup)
    {
      AddCodersProccess(coderGroup, Status.Grouped);

      _coderRepository.UpdateCodersGroup(coderGroup);
    }

    public async Task AddCodersSelected(CoderGroupDto coderGroup)
    {
      AddCodersProccess(coderGroup, Status.Selected);

      // We must to get coders that wasn't selected and put active again
      IEnumerable<CoderStatusHistory> oldGroupedHistory = await GetCompanyCoders(coderGroup.GroupId, Status.Grouped);

      foreach(CoderStatusHistory coderStatusHistory in oldGroupedHistory)
      {
        coderStatusHistory.IdGroup = "";
        coderStatusHistory.Status = Status.Active.ToString();
        Add(coderStatusHistory);
      }

      await _coderRepository.UpdateCodersSelected(coderGroup);
    }

    private IEnumerable<CoderStatusHistory> GetUniqueLastRegisters(BsonDocument filter)
    {
      var query = _mongoCollection.Aggregate()
      .SortByDescending(p => p.Date)
      .Group(p => p.IdCoder, g => new
      { 
        Data = g.First()
      })
      .Match(filter)
      .ToList();
          
      IEnumerable<CoderStatusHistory> group = query.Select(r => r.Data);
          
      return group;
    }

    private void AddCodersProccess(CoderGroupDto coderGroup, Status status)
    {
      List<string> coderIdList = coderGroup.CoderList;

      foreach(string coderId in coderIdList)
      {
        CoderStatusHistory coderStatusHistory = new CoderStatusHistory()
        {
          IdCoder = coderId,
          IdGroup = coderGroup.GroupId,
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
  }
}
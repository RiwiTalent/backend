using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Filters;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.Services.Repository
{
  public class CoderStatusHistoryRepository : ICoderStatusHistoryRepository
  {
    private readonly IMongoCollection<CoderStatusHistory> _mongoCollection;
    private readonly IMapper _mapper;
    private readonly ICoderRepository _coderRepository;
    private readonly IGroupCoderRepository _groupRepository;

    public CoderStatusHistoryRepository
    (
      MongoDbContext context,
      IMapper mapper,
      ICoderRepository coderRepository,
      IGroupCoderRepository groupRepository
    )
    {
      _mongoCollection = context.CoderStatusHistories;
      _mapper = mapper;
      _coderRepository = coderRepository;
      _groupRepository = groupRepository;
    }

    public async Task<CoderHistoryDto> GetCoderHistoryById(string coderId)
    {
      List<CoderStatusHistory> coderHistory = await _mongoCollection.Find(x => x.IdCoder == coderId)
        .ToListAsync();


      Coder coder = await _coderRepository.GetCoderId(coderId);

      CoderHistoryDto historyCoder = new CoderHistoryDto()
      {
        Name = coder.FirstName,
        GroupList = new List<Details>()
      };

      foreach(CoderStatusHistory history in coderHistory)
      {
        GroupDetailsDto groupCoder = await _groupRepository.GetGroupInfoById(history.IdGroup);

        if(groupCoder == null)
          continue;

        historyCoder.GroupList.Add(Details.CreateDetails(groupCoder.Name, history.Status));
      }

      return historyCoder;
    }

    public async Task<CoderHistoryDto> GetGroupHistoryById(string groupId)
    {
      List<CoderStatusHistory> coderHistory = await _mongoCollection.Find(x => x.IdGroup == groupId)
        .ToListAsync();


      GroupDetailsDto group = await _groupRepository.GetGroupInfoById(groupId);

      CoderHistoryDto historyCoder = new CoderHistoryDto()
      {
        Name = group.Name,
        CoderList = new List<Details>()
      };

      foreach(CoderStatusHistory history in coderHistory)
      {
        Coder coder = await _coderRepository.GetCoderId(history.IdCoder);

        if(coder == null)
          continue;

        string coderName = $"{coder.FirstName} {coder.FirstLastName}";

        historyCoder.CoderList.Add(Details.CreateDetails(coderName, history.Status));
      }

      return historyCoder;
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

    public async Task AddCodersGrouped(CoderGroupDto coderGroup)
    {

      try
      {
        await AddCodersProccess(coderGroup, Status.Grouped);

        await _coderRepository.UpdateCodersGroup(coderGroup);
      }
      catch (StatusError.CoderAlreadyInGroup ex)
      {
        throw new StatusError.CoderAlreadyInGroup(ex.Message);
      }
      catch (Exception ex)
      {
        throw new Exception("Ocurrió un error al agregar coders al grupo.", ex);
      }

      
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

    public async Task DeleteCoderGroup(string coderId)
    {
      CoderStatusHistory coderStatus = new CoderStatusHistory()
      {
        IdCoder = coderId,
        IdGroup = "",
        Status = Status.Active.ToString()
      };

      Add(coderStatus);
      _coderRepository.DeleteCoderGroup(coderId);
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

    private async Task AddCodersProccess(CoderGroupDto coderGroup, Status status)
    {
      List<string> coderIdList = coderGroup.CoderList;
 
      foreach(string coderId in coderIdList)
      {
        CoderStatusHistory coderStatusHistory = new CoderStatusHistory()
        {
          IdCoder = coderId,
          IdGroup = coderGroup.GroupId.ToString(),
          Status = status.ToString()
        };

        Add(coderStatusHistory);

        var coder = await _coderRepository.FindCoderById(coderId);

        if (coder != null)
        {
          if (coder.GroupId == null)
          {
              coder.GroupId = new List<string>();
          }

          if (!coder.GroupId.Contains(coderGroup.GroupId.ToString()))
          {

            coder.GroupId.Add(coderGroup.GroupId.ToString());

            await _coderRepository.Update(coder);
          }
          else
          {
            throw new StatusError.CoderAlreadyInGroup($"El coder con Id {coderId} ya está en el grupo {coderGroup.GroupId}.");
          }
        }
        else
        {
          throw new Exception("El coder con Id no fue encontrado.");
        }
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
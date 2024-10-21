using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities.Enums;
using RiwiTalent.Infrastructure.Filters;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Infrastructure.Persistence.Repository
{
  #pragma warning disable
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
      if(coder == null)
        throw new StatusError.ObjectIdNotFound("Coder not found");


      if (coder == null)
      {
          throw new KeyNotFoundException($"Coder with ID {coderId} not found.");
      }

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
      var coderHistory = await _mongoCollection.Find(x => x.IdGroup == groupId)
        .ToListAsync();


      GroupDetailsDto group = await _groupRepository.GetGroupInfoById(groupId);

      if (group == null)
      {
        throw new KeyNotFoundException($"Group with ID {groupId} not found.");
      }

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
      // we get both status, such as agruped and selecteds.
      BsonDocument filter;

      if(status == Status.Activo)
      {
        filter = MongoFilter.FilterByCompany(companyId);
        return GetUniqueLastRegisters(filter);
      }

      filter = MongoFilter.FilterByCompany(companyId, status);
      return GetUniqueLastRegisters(filter);
    }

    public async Task AddCodersAgrupado(CoderGroupDto coderGroup)
    {

      try
      {
        await AddCodersProccess(coderGroup, Status.Agrupado);

        await _coderRepository.UpdateCodersGroup(coderGroup);
      }
      catch (StatusError.CoderAlreadyInGroup ex)
      {
        throw new StatusError.CoderAlreadyInGroup(ex.Message);
      }
      catch (Exception ex)
      {
        throw new Exception($"An error occurred while adding coder to group. {ex.Message}");
      }

      
    }

    public async Task AddCodersSeleccionado(CoderGroupDto coderGroup)
    {
      await AddCodersProccess(coderGroup, Status.Seleccionado);

      // We must to get coders that wasn't selected and put active again
      IEnumerable<CoderStatusHistory> oldAgrupadoHistory = await GetCompanyCoders(coderGroup.GroupId, Status.Agrupado);

      foreach(CoderStatusHistory coderStatusHistory in oldAgrupadoHistory)
      {
        coderStatusHistory.IdGroup = null;
        coderStatusHistory.Status = Status.Activo.ToString();
        Add(coderStatusHistory);
      }

      await _coderRepository.UpdateCodersSeleccionado(coderGroup);
    }

    public async Task DeleteCoderGroup(string coderId, string groupId)
    {
      
      CoderStatusHistory coderStatus = new CoderStatusHistory()
      {
        IdCoder = coderId,
        IdGroup = groupId,
        Status = Status.Inactivo.ToString()
      };

      Add(coderStatus);
      _coderRepository.DeleteCoderOfGroup(coderId, groupId);
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

          }

          coderStatusHistory.Status = status.ToString();
          await _coderRepository.Update(coder);
        }
        else
        {
          throw new Exception("The coder with id not found");
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
using MongoDB.Bson;
using RiwiTalent.Models;
using RiwiTalent.Models.Enums;

namespace RiwiTalent.Utils.Filters
{
  public static class MongoFilter
  {
    public static BsonDocument FilterByCompany(string companyId)
    {
      BsonDocument filter = new BsonDocument
      {
        { $"Data.{nameof(CoderStatusHistory.IdGroup)}", companyId }
      };

      return filter;
    }

    public static BsonDocument FilterByCompany(string companyId, Status status)
    {
      BsonDocument filter = new BsonDocument
      {
        // { $"Data.{nameof(CoderStatusHistory.IdGroup)}", companyId },
        { $"Data.{nameof(CoderStatusHistory.Status)}", status.ToString() }
      };

      filter.AddRange(FilterByCompany(companyId));

      return filter;
    } 
  }
}
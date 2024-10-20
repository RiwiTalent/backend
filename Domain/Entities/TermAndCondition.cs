using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RiwiTalent.Domain.Entities
{
    public class TermAndCondition
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Content { get; set; }
        public DateTime Clicked_Date { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
        public bool Accepted { get; set; }
        public int Version { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GroupId { get; set; }
        public string? AcceptedEmail { get; set; }
        public string? CreatorEmail { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RiwiTalent.Domain.Entities
{
    public class CoderStatusHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdCoder { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdGroup { get; set; }
        public string? Status { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? Date {get; set;}
    }
}
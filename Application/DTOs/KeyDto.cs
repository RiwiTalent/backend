using MongoDB.Bson.Serialization.Attributes;

namespace RiwiTalent.Application.DTOs
{
    public class KeyDto
    {
        [BsonId]
        [BsonIgnore]
        public string? Id { get; set; }
        public string? AssociateEmail { get; set; }
        public string? Key { get; set; }
    }
}
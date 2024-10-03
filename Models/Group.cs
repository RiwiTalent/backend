using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Models
{
    public class Group
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? AssociateEmail { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Expiration_At { get; set; }
        public DateTime? Deleted_At { get; set; }
        public List<Coder>? Coders { get; set; }
        public List<ExternalKey>? ExternalKeys { get; set; }
    }
}
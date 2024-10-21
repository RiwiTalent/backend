using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Application.DTOs
{
    public class GroupDetailsDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime Created_At { get; set; }
        public string? CreatedBy { get; set; }
        public string? AssociateEmail { get; set; }
        public bool? AcceptedTerms { get; set; }
        public List<ExternalKey>? ExternalKeys { get; set; } 
        public List<CoderDto>? Coders {get; set;}
    }
}
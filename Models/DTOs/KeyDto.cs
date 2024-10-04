using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace RiwiTalent.Models.DTOs
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
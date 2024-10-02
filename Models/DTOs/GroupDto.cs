using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RiwiTalent.Models.DTOs
{
    public class GroupDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AssociateEmail { get; set; }
    }
}
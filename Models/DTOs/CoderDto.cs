using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using RiwiTalent.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace RiwiTalent.Models.DTOs
{
    public class CoderDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? FirstLastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? ProfessionalDescription { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public string? Phone { get; set; }
        public int Age { get; set; }
        public float AssessmentScore { get; set; }
        public string? Cv { get; set; }
        public string? Status { get; set; }
        public string? Stack { get; set; }
        public StandarRiwi? StandarRiwi { get; set; }
        public List<Skill>? Skills { get; set; }
        public LanguageSkill? LanguageSkills { get; set; }
    }
}
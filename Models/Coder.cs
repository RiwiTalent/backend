using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RiwiTalent.Models;

namespace MongoDb.Models
{
    public class Coder
    {
        public static object Firstname { get; internal set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? FirstLastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public int Age { get; set; }

        public string? Cv { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date_Creation { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date_Update { get; set; }
        public string? Status { get; set; }
        public string? Stack { get; set; }

        public StandarRiwi? StandarRiwi { get; set; }

        public List<Skill>? Skills { get; set; }
        public LanguageSkill? LanguageSkills { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? GroupId { get; set; }//FK

        
    }
}
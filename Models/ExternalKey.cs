namespace RiwiTalent.Models
{
    public class ExternalKey
    {
        public string? Url { get; set; }
        public string? Key { get; set; }
        public string? Status { get; set; }
        public DateTime Date_Creation { get; set; }
        public DateTime Date_Expiration { get; set; }
    }
}
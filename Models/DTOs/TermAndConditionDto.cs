namespace RiwiTalent.Models.DTOs
{
    public class TermAndConditionDto
    {
        public string Id { get; set; } // O el tipo correcto para tu Id (ej. ObjectId en MongoDB)
        public string Content { get; set; }
        public DateTime Clicked_Date { get; set; }
        public bool IsActive { get; set; }
        public bool Accepted { get; set; }
        public int Version { get; set; }
        public string? GroupId { get; set; }
        public string? AcceptedEmail { get; set; }
        public string? CreatorEmail { get; set; }
    }
}

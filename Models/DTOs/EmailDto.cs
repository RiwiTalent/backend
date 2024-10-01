namespace RiwiTalent.Models.DTOs{
    public class EmailDto{
        public List<string> Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
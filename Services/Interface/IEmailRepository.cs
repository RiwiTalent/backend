using MimeKit;


namespace RiwiTalent.Services.Interface
{

    public interface IEmailRepository{
        void SendEmailTest(string Id);
        void SendTermsAndConditions(string Name, string EmailRecipient);
        void SendEmailStaff(string Name, string Email, string Message, string groupId);
        void SendEmailAll(MimeMessage mimeMessage);
    }



}
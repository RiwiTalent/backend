using MimeKit;


namespace RiwiTalent.Domain.Services.Interface.Emails
{

    public interface IEmailRepository{
        Task SendEmailTest(string Id);
        Task SendTermsAndConditions(string Name, string EmailRecipient);
        Task SendEmailStaff(string Name, string Email, string Message, string groupId);
        Task SendEmailAll(MimeMessage mimeMessage);
    }



}
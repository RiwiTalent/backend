using MimeKit;

namespace RiwiTalent.Domain.Services.Interface.Emails
{
    public interface IEmailSelectedRepository
    {
        Task SendEmail(MimeMessage message);
        Task SendCodersSelectedStaff(string Name, string Email, string groupId);
        Task SendEmailExternal(string Name, string Email);
        Task SendEmailAll(string id);
    }
}
using MimeKit;

namespace RiwiTalent.Services.Interface
{
    public interface IEmailSelectedRepository
    {
        void SendEmail(MimeMessage message);
        void SendCodersSelectedStaff(string Name, string Email, string groupId);
        void SendEmailExternal(string Name, string Email);
        void SendEmailAll(string id);
    }
}
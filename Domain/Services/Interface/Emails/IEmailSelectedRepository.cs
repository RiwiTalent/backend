using MimeKit;

namespace RiwiTalent.Domain.Services.Interface.Emails
{
    public interface IEmailSeleccionadoRepository
    {
        Task SendEmail(MimeMessage message);
        Task SendCodersSeleccionadoStaff(string Name, string Email, string groupId);
        Task SendEmailExternal(string Name, string Email);
        Task SendEmailAll(string id);
    }
}
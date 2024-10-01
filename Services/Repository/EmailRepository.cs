using RiwiTalent.Models.DTOs;
using MimeKit.Text;
using RiwiTalent.Services.Interface;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace RiwiTalent.Services.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
    
        public EmailRepository(IConfiguration config)
        {
            _config = config;
        }

        



        
        public void SendEmail(EmailDto email)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));

            message.To.Add(MailboxAddress.Parse(email.Sender));               
        
            message.Subject = email.Subject;
            message.Body = new TextPart("text/plain")
            {
                Text = email.Body
            };

            var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );


            smtp.Authenticate( _config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(message);
            smtp.Disconnect(true);

            
        }





    }
}

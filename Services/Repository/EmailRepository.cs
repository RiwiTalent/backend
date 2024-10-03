using RiwiTalent.Models.DTOs;
using MimeKit.Text;
using RiwiTalent.Services.Interface;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.IO;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Utils.MailKit;

namespace RiwiTalent.Services.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
        private readonly SendFile _sendFile;
    
        public EmailRepository(IConfiguration config, SendFile sendFile)
        {
            _config = config;
            _sendFile = sendFile;
        }

    
        
        public void SendEmail(EmailDto email)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));

            // Add multiple recipients 
            foreach (var recipient in email.Recipients)
            {
                message.To.Add(MailboxAddress.Parse(recipient));
                message.Subject = "Hola";
            }

            message.Subject = email.Subject;
            
            var bodyBuilder = new BodyBuilder
            {
                TextBody = email.Body
            };
            
            var path = "Utils/Resources/TermsAndConditions.pdf";
            try
            {
                var attachment = _sendFile.GetFileTerms(path);
                bodyBuilder.Attachments.Add(attachment);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"Error attaching file: {ex.Message}");
            }

            message.Body = bodyBuilder.ToMessageBody();   

            try
            {
                using (var smtp = new SmtpClient())
                {
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
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                throw new Exception(problemDetails.Status.Value.ToString());
            }

            
        }





    }
}

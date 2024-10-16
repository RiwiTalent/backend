using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MongoDB.Driver;
using RiwiTalent.Domain.Services.Interface.Emails;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Infrastructure.ExternalServices.MailKit;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Infrastructure.Persistence.Emails
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Group> _mongoCollection;
        private readonly SendFile _sendFile;
        
    
        public EmailRepository(IConfiguration config, SendFile sendFile, MongoDbContext context)
        {
            _config = config;
            _sendFile = sendFile;
            _mongoCollection = context.Groups;
        }

        //Here we realize emails to staff and companies partners, when the company partner has accepted terms and conditions


        public async Task SendEmailAll(MimeMessage message)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(
                        _config.GetSection("Email:Host").Value,
                        Convert.ToInt32(_config.GetSection("Email:Port").Value),
                        SecureSocketOptions.SslOnConnect
                    );


                    await smtp.AuthenticateAsync( _config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (SmtpCommandException smtpEx)
            {
                throw new Exception($"Error smtp sending email {smtpEx.Message}");
            }
            catch (SmtpProtocolException protocolEx)
            {
                throw new Exception($"Error protocol smtp: {protocolEx.Message}");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                throw new Exception(problemDetails.Status.Value.ToString());
            }
           
        }

        public async Task SendTermsAndConditions(string Name, string EmailRecipient)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Riwi", "riwitalen@gmail.com"));
            message.To.Add(new MailboxAddress(Name, EmailRecipient));
            message.Subject = "Terms and Conditions";


            var htmlTemplatePath = "Infrastructure/ExternalServices/MailKit/Templates/email.html";
            string htmlTemplate;
            try
            {
                htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading template to message {ex.Message}");
            }
            

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlTemplate,
                TextBody =  "Adjunto pdf terminos y condiciones"
            };

            var path = "Utils/Resources/TermsAndConditions.pdf";
            try
            {
                var attachment = await _sendFile.GetFileTermsAsync(path);
                bodyBuilder.Attachments.Add(attachment);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"Error attaching file: {ex.Message}");
            }

            message.Body = bodyBuilder.ToMessageBody();

            await SendEmailAll(message);
        }

        public async Task SendEmailStaff(string Name, string Email, string Message, string groupId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Riwi", "riwitalen@gmail.com"));
            message.To.Add(new MailboxAddress(Name, Email));
            message.Subject = "Información Importante";

            var searchGroup = await _mongoCollection.Find(g => g.Id == groupId).FirstOrDefaultAsync();

            if(searchGroup == null)
                throw new StatusError.ObjectIdNotFound("The document not found");

            var htmlTemplatePath = "Infrastructure/ExternalServices/MailKit/Templates/email_staff.html";
            string htmlTemplate;
            try
            {
                htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath);
                htmlTemplate = htmlTemplate.Replace("{empresa}", searchGroup.Name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading template to message {ex.Message}");
            }

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlTemplate,
                TextBody =  Message
            };

            message.Body = bodyBuilder.ToMessageBody();

            await SendEmailAll(message);
        }



        public async Task SendEmailTest(string id)
        {
            // we realize search comparing objectId
            var tech = await _mongoCollection.Find(t => t.Id.ToString() == id).FirstOrDefaultAsync();

            if(tech == null)
                throw new StatusError.ObjectIdNotFound("The document Technology not found");

            await SendTermsAndConditions("External", tech.AssociateEmail);

            var groupId = tech.Id;

            await SendEmailStaff("Staff", tech.CreatedBy, "Términos y Condiciones", groupId.ToString());

        }


    }
}

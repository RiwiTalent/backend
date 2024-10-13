using RiwiTalent.Services.Interface;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Utils.MailKit;
using RiwiTalent.Models;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;

namespace RiwiTalent.Services.Repository
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


        public void SendEmailAll(MimeMessage message)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(
                        _config.GetSection("Email:Host").Value,
                        Convert.ToInt32(_config.GetSection("Email:Port").Value),
                        SecureSocketOptions.SslOnConnect
                    );


                    smtp.Authenticate( _config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
                    smtp.Send(message);
                    smtp.Disconnect(true);
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

        public void SendTermsAndConditions(string Name, string EmailRecipient)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Riwi", "riwitalen@gmail.com"));
            message.To.Add(new MailboxAddress(Name, EmailRecipient));
            message.Subject = "Terms and Conditions";


            var htmlTemplatePath = "Utils/Templates/email.html";
            string htmlTemplate;
            try
            {
                htmlTemplate = File.ReadAllText(htmlTemplatePath);
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
                var attachment = _sendFile.GetFileTerms(path);
                bodyBuilder.Attachments.Add(attachment);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"Error attaching file: {ex.Message}");
            }

            message.Body = bodyBuilder.ToMessageBody();

            SendEmailAll(message);
        }

        public void SendEmailStaff(string Name, string Email, string Message, string groupId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Riwi", "riwitalen@gmail.com"));
            message.To.Add(new MailboxAddress(Name, Email));
            message.Subject = "Información Importante";

            var searchGroup = _mongoCollection.Find(g => g.Id == groupId).FirstOrDefault();

            if(searchGroup == null)
                throw new StatusError.ObjectIdNotFound("The document not found");

            var htmlTemplatePath = "Utils/Templates/email_staff.html";
            string htmlTemplate;
            try
            {
                htmlTemplate = File.ReadAllText(htmlTemplatePath);
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

            SendEmailAll(message);
        }



        public void SendEmailTest(string id)
        {
            // we realize search comparing objectId
            var tech = _mongoCollection.Find(t => t.Id.ToString() == id).FirstOrDefault();

            if(tech == null)
                throw new StatusError.ObjectIdNotFound("The document Technology not found");

            SendTermsAndConditions("External", tech.AssociateEmail);

            var groupId = tech.Id;

            SendEmailStaff("Staff", tech.CreatedBy, "Términos y Condiciones", groupId.ToString());

        }


    }
}

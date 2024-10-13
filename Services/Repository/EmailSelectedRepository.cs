using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Utils.MailKit;
using System.Text;
using RiwiTalent.Utils;
using RiwiTalent.Models.Enums;

namespace RiwiTalent.Services.Repository
{
    public class EmailSelectedRepository : IEmailSelectedRepository
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Coder> _coder;
        private readonly IMongoCollection<Group> _group;
        private readonly SendFile _sendFile;
        
    
        public EmailSelectedRepository(IConfiguration config, SendFile sendFile, MongoDbContext context)
        {
            _config = config;
            _sendFile = sendFile;
            _coder = context.Coders;
            _group = context.Groups;
        }

        //Here we realize emails to staff and companies partners, with diferent messages when thare is coders selected

        public void SendEmail(MimeMessage message)
        {
            try
            {
                using(var smtp = new MailKit.Net.Smtp.SmtpClient())
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
                Console.WriteLine($"{smtpEx.Message}");
                throw new Exception($"Error smtp sending email {smtpEx.Message}");
            }
            catch (SmtpProtocolException protocolEx)
            {
                Console.WriteLine($"{protocolEx.Message}");
                throw new Exception($"Error protocol smtp: {protocolEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error enviando el correo: {ex.Message}");
                var problemDetails = StatusError.CreateInternalServerError(ex);
                throw new Exception(problemDetails.Status.Value.ToString());
            }
        }

        public void SendCodersSelectedStaff(string Name, string Email, string groupId)
        {
            var findGroup = _group.Find(g => g.Id == groupId).FirstOrDefault();
            var findSelectedCoders = _coder.Find(c => c.GroupId.Contains(groupId) && c.Status == Status.Selected.ToString()).ToList();

            if(findGroup == null)
                throw new StatusError.ObjectIdNotFound("The document not found");

            if(findSelectedCoders == null || !findSelectedCoders.Any())
                throw new StatusError.ObjectIdNotFound("No coders found for the groupId or the coder has not selected yet");

            EmailCoderSelected emailCoderSelected = new EmailCoderSelected();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Riwi", "riwitalen@gmail.com"));
            message.To.Add(new MailboxAddress(Name, Email));
            message.Subject = $"{findGroup.Name} Inició Proceso";

            
            var builder = new BodyBuilder();
            StringBuilder emailExternal = new StringBuilder();

            foreach (var coder in findSelectedCoders)
            {
                string emailTemplate = emailCoderSelected.GenerateTemplate(coder, emailCoderSelected.template);
                emailExternal.Append(emailTemplate);
            }
            
                
            builder.HtmlBody = emailExternal.ToString();
            message.Body = builder.ToMessageBody(); 
           

            SendEmail(message);

        }

        public void SendEmailExternal(string Name, string Email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Riwi", "riwitalen@gmail.com"));
            message.To.Add(new MailboxAddress(Name, Email));
            message.Subject = "Has Iniciado Proceso";

            var htmlTemplatePath = "Utils/Templates/email_coder_selected_external.html";
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
                TextBody = "Felicidades, has iniciado un proceso para adquirir el mejor talento"
            };

            message.Body = bodyBuilder.ToMessageBody();
            SendEmail(message);
        }

        public void SendEmailAll(string id)
        {
            var group = _group.Find(g => g.Id == id).FirstOrDefault();
            if(group == null)
                throw new StatusError.ObjectIdNotFound("The document Technology not found");


            var groupId = group.Id;

            SendCodersSelectedStaff("Staff", group.CreatedBy, groupId.ToString());
            SendEmailExternal("External", group.AssociateEmail);
    
        }

        
    }
}
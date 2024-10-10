using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Utils.MailKit;

namespace RiwiTalent.Services.Repository
{
    public class EmailSelectedRepository : IEmailSelectedRepository
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Coder> _mongoCollection;
        private readonly SendFile _sendFile;
        
    
        public EmailSelectedRepository(IConfiguration config, SendFile sendFile, MongoDbContext context)
        {
            _config = config;
            _sendFile = sendFile;
            _mongoCollection = context.Coders;
        }

        

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
                throw new Exception($"Error smtp sending email {smtpEx.Message}");
            }
            catch (SmtpProtocolException protocolEx)
            {
                throw new Exception($"Error protocol smtp: {protocolEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error enviando el correo: {ex.Message}");
                var problemDetails = StatusError.CreateInternalServerError(ex);
                throw new Exception(problemDetails.Status.Value.ToString());
            }
        }

        public void SendCodersSelected()
        {
            throw new NotImplementedException();

        }

        
    }
}
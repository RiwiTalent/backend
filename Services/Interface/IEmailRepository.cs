using MimeKit;
using MongoDB.Bson;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;


namespace RiwiTalent.Services.Interface{

    public interface IEmailRepository{
        /* void SendEmail(EmailDto email); */
        void SendEmailTest(string Id);
        void SendTermsAndConditions(string Name, string EmailRecipient);
        void SendEmailStaff(string Name, string Email, string Message);
        void SendEmailAll(MimeMessage mimeMessage);
    }



}
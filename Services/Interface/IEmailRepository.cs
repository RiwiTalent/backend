using RiwiTalent.Models.DTOs;


namespace RiwiTalent.Services.Interface{

    public interface IEmailRepository{
        void SendEmail(EmailDto email);
    }



}
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.Email
{
    public class EmailController : Controller
    {
        private readonly IEmailRepository _emailRepository;
        public EmailController(IEmailRepository emailRepostory)
        {
            _emailRepository = emailRepostory;
        }

        [HttpPost("api/email/send")]
        public IActionResult SendEmail(EmailDto email)
        {

            _emailRepository.SendEmail(email);
            return Ok();
        }


    }

}
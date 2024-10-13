using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.Email
{
    public class EmailController : Controller
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSelectedRepository _emailSelectedRepository;
        public EmailController(IEmailRepository emailRepostory, IEmailSelectedRepository emailSelectedRepository)
        {
            _emailRepository = emailRepostory;
            _emailSelectedRepository = emailSelectedRepository;
        }

        [HttpPost("api/email-process")]
        public IActionResult SendEmail([FromQuery] string id)
        {
            try
            {
                _emailSelectedRepository.SendEmailAll(id);
                return Ok("The email has beend delivered"); 
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpPost("email/send")]
        public IActionResult SendEmailTesting([FromQuery] string Id)
        {
            try
            {
                _emailRepository.SendEmailTest(Id);
                return Ok("The email has beend delivered"); 
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }



    }

}
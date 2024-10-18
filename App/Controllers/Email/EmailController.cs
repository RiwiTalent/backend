using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Interface.Emails;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Email
{
    public class EmailController : Controller
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSeleccionadoRepository _emailSeleccionadoRepository;
        public EmailController(IEmailRepository emailRepostory, IEmailSeleccionadoRepository emailSeleccionadoRepository)
        {
            _emailRepository = emailRepostory;
            _emailSeleccionadoRepository = emailSeleccionadoRepository;
        }

        [HttpPost("email/company-process")]
        public async Task<IActionResult> SendEmail([FromQuery] string id)
        {
            try
            {
                await _emailSeleccionadoRepository.SendEmailAll(id);
                return Ok("The email has beend delivered"); 
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
                throw;
            }
        }

        [HttpPost("email/accept-terms")]
        public async Task<IActionResult> SendEmailTesting([FromQuery] string Id)
        {
            try
            {
                await _emailRepository.SendEmailTest(Id);
                return Ok("The email has beend delivered"); 
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
                throw;
            }
        }



    }

}
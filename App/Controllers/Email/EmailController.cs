using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

        /* [HttpPost("api/email/send")]
        public IActionResult SendEmail(EmailDto email)
        {
            try
            {
                _emailRepository.SendEmail(email);
                return Ok(); 
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        } */

        [HttpPost("email/send")]
        public IActionResult SendEmailTesting()
        {
            try
            {
                _emailRepository.SendEmailTest();
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
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{
    public class TermAndConditionCreateController : ControllerBase
    {
        private readonly ITermAndConditionRepository _termAndConditionRepository;

        public TermAndConditionCreateController(ITermAndConditionRepository termAndConditionRepository)
        {
            _termAndConditionRepository = termAndConditionRepository;
        }

        [HttpPost("terms")]
        public async Task<ActionResult> AcceptUserTerms(TermAndConditionDto termAndConditionDto) 
        {
            if (termAndConditionDto == null)
            {
                return BadRequest("Invalid data."); 
            }

            try
            {
                await _termAndConditionRepository.Add(termAndConditionDto);
                return Ok("Terms and conditions have been created.");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
        }
    }
}

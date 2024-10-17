using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Services.Interface.Terms;
using RiwiTalent.Shared.Exceptions;

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
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
            }
        }
    }
}

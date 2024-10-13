using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{

    public class TermAndConditionUpdateController : ControllerBase
    {
        private readonly ITermAndConditionRepository _termAndConditionRepository;

        public TermAndConditionUpdateController(ITermAndConditionRepository termAndConditionRepository)
        {
            _termAndConditionRepository = termAndConditionRepository;
        }

        
        [HttpPut("terms")]
        public async Task<ActionResult> UpdateTerms(TermAndConditionDto updatedTermsDto)
        {
            try
            {
                await _termAndConditionRepository.UpdateTermsAsync(updatedTermsDto);
                return Ok("Terms have been updated.");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
        }
    }
}

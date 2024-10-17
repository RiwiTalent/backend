using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Services.Interface.Terms;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{

    public class TermAndConditionUpdateController : ControllerBase
    {
        private readonly ITermAndConditionRepository _termAndConditionRepository;

        public TermAndConditionUpdateController(ITermAndConditionRepository termAndConditionRepository)
        {
            _termAndConditionRepository = termAndConditionRepository;
        }

        
        [HttpPut("terms/{id}")]
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
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Terms;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{

    public class TermAndConditionController : ControllerBase
    {
        private readonly ITermAndConditionRepository _termAndConditionRepository;

        public TermAndConditionController(ITermAndConditionRepository termAndConditionRepository)
        {
            _termAndConditionRepository = termAndConditionRepository;
        }

        // endpoint to list all terms
        [HttpGet("terms")]
        public async Task<ActionResult<List<TermAndCondition>>> GetAllTerms()
        {
            try
            {
                var terms = await _termAndConditionRepository.GetAllTermsAsync();
                return Ok(terms);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
            }
        }

        // Endpoint para obtener términos por correo electrónico
        [HttpGet("terms/{email}")]
        public async Task<ActionResult<TermAndCondition>> GetTermsByEmail(string email)
        {
            try
            {
                var terms = await _termAndConditionRepository.GetTermsByEmailAsync(email);
                if (terms == null)
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound($"No terms found for this email.", instance));
                }
                return Ok(terms);
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

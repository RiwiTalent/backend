using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Models;
using System;
using System.Threading.Tasks;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{

    public class TermAndConditionController : ControllerBase
    {
        private readonly ITermAndConditionRepository _termAndConditionRepository;

        public TermAndConditionController(ITermAndConditionRepository termAndConditionRepository)
        {
            _termAndConditionRepository = termAndConditionRepository;
        }

        // Nuevo endpoint para listar todos los términos
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
                return StatusCode(problemDetails.Status.Value, problemDetails);
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
                    return NotFound("No terms found for this email.");
                }
                return Ok(terms);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
        }

    }
}

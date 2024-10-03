using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Models;
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

        // Endpoint para aceptar términos
        [HttpPost("terms")]
        public async Task<ActionResult> AcceptUserTerms()
        {
            try
            {
                await _termAndConditionRepository.Add();
                return Ok("Data has been updated");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
        }

    }
}

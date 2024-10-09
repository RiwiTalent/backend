using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{
    //[Authorize]
    public class TermAndConditionCreateController : Controller
    {
        private readonly ITermAndConditionRepository _termAndConditionRepository;
        public TermAndConditionCreateController(ITermAndConditionRepository termAndConditionRepository)
        {
            _termAndConditionRepository = termAndConditionRepository;
        }

        //endpoint
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
                throw;
            }
        }
    }
}

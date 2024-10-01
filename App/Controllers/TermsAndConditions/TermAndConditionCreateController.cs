using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.App.Controllers.TermsAndConditions
{
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
            await _termAndConditionRepository.Add();
            return Ok("Data has been updated");
        }
    }
}
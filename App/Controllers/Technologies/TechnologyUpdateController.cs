using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.App.Controllers.Technologies
{
    public class TechnologyUpdateController : Controller
    {
        private readonly ITechnologyRepository _technologyRepository;
        public TechnologyUpdateController(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        //endpoint
        [HttpPatch("technology")]
        public async Task<ActionResult> UpdateTechnology(Technology technology)
        {
            await _technologyRepository.Update(technology);
            return Ok();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.App.Controllers.Technologies
{
    public class TechnologiesController : Controller
    {
        private readonly ITechnologyRepository _technologyRepository;
        public TechnologiesController(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        //endpoint
        [HttpGet]
        [Route("technologies")]
        public async Task<IActionResult> Get()
        {
            var techs = await _technologyRepository.GetTechnologies();
            return Ok(techs);
        }   
    }
}
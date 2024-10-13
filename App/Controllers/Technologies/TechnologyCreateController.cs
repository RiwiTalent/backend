using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.Technologies
{
    public class TechnologyCreateController : Controller
    {
        private readonly ITechnologyRepository _technologyRepository;
        public TechnologyCreateController(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        //endpoint
        [HttpPost]
        [Route("technologies")]
        public IActionResult Post(Technology technology)
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                _technologyRepository.Add(technology);
                return Ok();
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }            
        }

        /* [HttpPost("technology")]
        public async Task<IActionResult> AddTech([FromQuery] string technologyId, [FromQuery] string newTechnology)
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                await _technologyRepository.AddTechnology(technologyId, newTechnology);
                return Ok("The new technology has been agregated");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }  
        } */
    }
}
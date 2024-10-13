using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

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
            try
            {
                await _technologyRepository.Update(technology);
                return Ok("The technologies has been updated"); 
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
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Technologies;
using RiwiTalent.Shared.Exceptions;

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
        [HttpPatch("technologies/{id}")]
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
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
                throw;
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Interface.Technologies;
using RiwiTalent.Shared.Exceptions;

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
            try
            {
                var techs = await _technologyRepository.GetTechnologies();

                return Ok(techs); 
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
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.App.Controllers.Coders
{
    public class CoderUpdateController : Controller
    {
        private readonly ICoderRepository _coderRepository;
        public CoderUpdateController(ICoderRepository coderRepository)
        {
            _coderRepository = coderRepository;
        }

        //Endpoint
        [HttpPut]
        [Route("riwitalent/updatecoder")]
        public async Task<IActionResult> UpdateCoder([FromBody] CoderDto coderDto)
        {
            if(coderDto == null)
            {
                return BadRequest(Utils.Exceptions.StatusError.CreateBadRequest());
            }

            try
            {
                await _coderRepository.Update(coderDto);
                return Ok("The coder has been updated the correct way");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }

        
    }
}
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

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
        [Route("coders")]
        public async Task<IActionResult> UpdateCoder(Coder coder)
        {
            if(coder == null)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                await _coderRepository.Update(coder);
                return Ok("The coder has been updated the correct way");
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

        [HttpPatch]
        [Route("coders/{id:length(24)}/reactivate")]
        public async Task<IActionResult> Reactivate(string id)
        {
            /* The function has the main principle of search by coder id
                and then update status the Inactive to Active
            */
            try
            {
                await _coderRepository.ReactivateCoder(id);
                return Ok(new { Message = "The status of coder has been updated to Active" });
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
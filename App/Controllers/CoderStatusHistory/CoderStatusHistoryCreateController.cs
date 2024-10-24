using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Coders
{
    public class CoderStatusHistoryCreateController : Controller
    {
        private readonly ICoderStatusHistoryRepository _coderStatusHistoryRepository;
        public CoderStatusHistoryCreateController(ICoderStatusHistoryRepository coderStatusHistoryRepository)
        {
            _coderStatusHistoryRepository = coderStatusHistoryRepository;
        }

        // [HttpPost]
        // [Route("riwitalent/addstatus")]
        // public IActionResult Post([FromBody] CoderStatusHistory coderHistory)
        // {
        //     if(coderHistory == null)
        //     {
        //         return BadRequest(Utils.Exceptions.StatusError.CreateBadRequest());
        //     }

        //     try
        //     {
        //         _coderStatusHistoryRepository.AddCodersAgrupado(coderHistory);
        //         return Ok("The coder has been created successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //         throw;
        //     }
        // }

        [HttpPost]
        [Route("coders/grouped")]
        public IActionResult AddCodersToGroup([FromBody] CoderGroupDto coderGroup)
        {
            try
            {
                _coderStatusHistoryRepository.AddCodersAgrupado(coderGroup);
                return Ok("List of coders sucessfully added to group");
            }   
            catch(StatusError.CoderAlreadyInGroup ex)
            {
                return BadRequest(new { message = ex.Message });
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

        [HttpPost]
        [Route("coders/selected")]
        public async Task<IActionResult> AddSeleccionadoCoders([FromBody] CoderGroupDto coderGroup)
        {
            try
            {
                await _coderStatusHistoryRepository.AddCodersSeleccionado(coderGroup);
                return Ok("List of coders sucessfully selected by company");
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
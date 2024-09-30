using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

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
        //         _coderStatusHistoryRepository.AddCodersGrouped(coderHistory);
        //         return Ok("The coder has been created successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //         throw;
        //     }
        // }

        [HttpPost]
        [Route("riwitalent/addCodersGrouped")]
        public IActionResult AddCodersToGroup([FromBody] CoderGroupDto coderGroup)
        {
            try
            {
                _coderStatusHistoryRepository.AddCodersGrouped(coderGroup);
                return Ok("List of coders sucessfully added to group");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpPost]
        [Route("riwitalent/updateCodersSelected")]
        public async Task<IActionResult> AddSelectedCoders([FromBody] CoderGroupDto coderGroup)
        {
            try
            {
                await _coderStatusHistoryRepository.AddCodersSelected(coderGroup);
                return Ok("List of coders sucessfully selected by company");
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
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers
{
    public class CoderStatusHistoryController : Controller
    {
        private readonly ICoderStatusHistoryRepository _coderStatusHistoryRepository;
        public CoderStatusHistoryController(ICoderStatusHistoryRepository coderStatusHistoryRepository)
        {
            _coderStatusHistoryRepository = coderStatusHistoryRepository;
        }

        //get all process history statues
        /*
            {
                {"jose", "celsia", "grouped"},
                {"laura", "celsia", "selected"}
                {"Omar", "", "Active"}
            }
        */
        [HttpGet]
        [Route("riwitalent/historyStatus")]
        public async Task<IActionResult> GetAllHistory()
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                var coders = await _coderStatusHistoryRepository.GetCodersHistoryStatus();
                return Ok(coders);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpGet]
        [Route("riwitalent/historyStatus/coder/{id}")]
        public async Task<IActionResult> GetCoderHistory(string id)
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                var coders = await _coderStatusHistoryRepository.GetCoderHistoryById(id);

                // JsonSerializerOptions options = new()
                // {
                //     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                // };

                // string forecastJson = JsonSerializer.Serialize<CoderHistoryDto>(coders, options);
                return Ok(coders);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpGet]
        [Route("riwitalent/historyStatus/group/{id}")]
        public async Task<IActionResult> GetGroupHistory(string id)
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                var coders = await _coderStatusHistoryRepository.GetGroupHistoryById(id);
                return Ok(coders);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        /*
            Permite listar los coders de un grupo segun el estado, ya sea agrupado o en 
            proceso
        */
        // [HttpGet]
        // [Route("riwitalent/groupCodersHistory/{groupId}")]
        // public async Task<IActionResult> GetByGroupId(string groupId, [FromQuery] Status status)
        // {
        //     if(!ModelState.IsValid)
        //     {
        //         return BadRequest(RiwiTalent.Utils.Exceptions.StatusError.CreateBadRequest());
        //     }

        //     try
        //     {
        //         var coders = await _coderStatusHistoryRepository.GetCompanyCoders(groupId, status);
        //         return Ok(coders);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //         throw;
        //     }
        // }
    }
}
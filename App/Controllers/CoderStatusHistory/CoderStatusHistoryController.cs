using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

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
        [Route("historystatuses")]
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
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
                throw;
            }
        }

        [HttpGet]
        [Route("historystatuses/coder/{id}")]
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

                /* JsonSerializerOptions options = new()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string forecastJson = JsonSerializer.Serialize<CoderHistoryDto>(coders, options); */
                return Ok(coders);
            }
            catch(KeyNotFoundException ex)
            {
                var problemDetails = StatusError.CreateNotFound(ex.Message, Guid.NewGuid().ToString());
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
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

        [HttpGet]
        [Route("historystatuses/group/{id}")]
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
            catch(KeyNotFoundException ex)
            {
                var problemDetails = StatusError.CreateNotFound(ex.Message, Guid.NewGuid().ToString());
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
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
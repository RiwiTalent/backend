using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers
{
    public class CodersController : Controller
    {
        private readonly ICoderRepository _coderRepository;
        public CodersController(ICoderRepository coderRepository)
        {
            _coderRepository = coderRepository;
        }

        //get all coders
        [HttpGet]
        [Route("riwitalent/coders")]
        public async Task<IActionResult> Get()
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                var coders = await _coderRepository.GetCoders();
                return Ok(coders);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }


        //Get all coders pagination
        [HttpGet]
        [Route("riwitalent/coders/page={page}")]
        public async Task<IActionResult> Get(int page = 1,int cantRegisters = 10)
        {
            /*The main idea of this method, is when the user list all coders can watch for pagination*/
            try
            {
                var coderPagination = await _coderRepository.GetCodersPagination(page, cantRegisters);

                if(coderPagination == null)
                {
                    var instance = Guid.NewGuid().ToString();
                    var problemDetails = StatusError.CreateBadRequest(instance);
                    return BadRequest(problemDetails);
                }
                return Ok(new 
                {
                    Page = page,
                    Registers = coderPagination.Count(),
                    Coders = coderPagination,
                    TotalPages = coderPagination.TotalPages,
                    PageBefore = coderPagination.PageBefore,
                    PageAfter = coderPagination.PageAfter
                });
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        //Get coder by id
        [HttpGet]
        [Route("riwitalent/{id}/coder")]
        public async Task<IActionResult> GetCoderById(string id)
        {
            try
            {
                var coder = await _coderRepository.GetCoderId(id);

                if (coder is null)
                {
                    /* return NotFound(new { message = $"Coder con ID {id} no fue encontrado." }); */
                    Utils.Exceptions.StatusError.CreateNotFound($"The coder with {id} not found");
                }

                return Ok(coder);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        //Get Coder by name
        [HttpGet]
        [Route("riwitalent/{name}/coders")]
        public async Task<IActionResult> GetCoderByName(string name)
        {
            try
            {
                var coder = await _coderRepository.GetCoderName(name);

                if (coder is null)
                {
                    return NotFound(StatusError.CreateNotFound($"The coder {name} not found."));
                }

                return Ok(coder);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }


        //Get coder by skill tecnical
        [HttpGet]
        [Route("RiwiTalent/skill/coder")]
        public async Task<IActionResult> GetCodersBySkill([FromQuery] List<string> skill)
        {
            try
            {
                var coders = await _coderRepository.GetCodersBySkill(skill);
                if (coders is null || !coders.Any())
                {
                    return NotFound(StatusError.CreateNotFound("There isn't coder with those languages."));
                }

                return Ok(coders);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
            
        }

        //Get coder by language level in english
        [HttpGet]
        [Route("RiwiTalent/coder/{language}/level")]
        public async Task<IActionResult> GetCodersByLanguage([FromQuery] string level, string language = "English")
        {
            try
            {
                var coders = await _coderRepository.GetCodersBylanguage(level);
                if (coders is null || !coders.Any())
                {
                    return NotFound(StatusError.CreateNotFound("There isn't coder with those language level."));
                }
                return Ok(coders);
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
